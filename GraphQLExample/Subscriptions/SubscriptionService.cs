using GraphQLExample.Subscriptions.Internal;
using GraphQLExample.Subscriptions.Messages;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace GraphQLExample.Subscriptions
{
    public sealed class SubscriptionService : ISubscriptionService, IAsyncDisposable
    {
        private readonly ConcurrentDictionary<Guid, IUntypedLocalSubscription> localSubscriptions = new();
        private readonly ConcurrentDictionary<Guid, SubscriptionEntry> clusterSubscriptions = new();
        private readonly SubscriptionOptions options;
        private readonly string senderId;
        private readonly ISubscriptionTransport transport;
        private readonly ISubscriptionEvaluator evaluator;
        private readonly ILogger<SubscriptionService> log;
        private readonly IDisposable transportSubscription;
        private readonly SimpleTimer cleanupTimer;

        // Just for testing.
        public IClock Clock { get; set; } = new DefaultClock();

        public SubscriptionService(
            ISubscriptionSenderProvider senderProvider,
            ISubscriptionTransport transport,
            ISubscriptionEvaluator evaluator,
            IOptions<SubscriptionOptions> options,
            ILogger<SubscriptionService> log)
        {
            this.transport = transport;
            this.evaluator = evaluator;
            this.options = options.Value;
            this.log = log;

            transportSubscription = transport.Subscribe(OnMessage);

            cleanupTimer = new SimpleTimer(_ =>
            {
                Cleanup();
                return Task.CompletedTask;
            }, options.Value.SubscriptionUpdateTime, log);

            senderId = senderProvider.Sender;
        }

        public ValueTask DisposeAsync()
        {
            transportSubscription.Dispose();

            return cleanupTimer.DisposeAsync();
        }

        public void Cleanup()
        {
            if (localSubscriptions.Count > 0)
            {
                // Only publish a message if there is at least one subscription.
                transport.Publish(new SubscriptionsAliveMessage
                {
                    SubscriptionIds = localSubscriptions.Keys.ToList(),
                    SourceId = senderId
                });
            }

            if (clusterSubscriptions.Count > 0)
            {
                var now = Clock.UtcNow;

                var numExpiredSubscriptions = 0;

                foreach (var entry in clusterSubscriptions.Values.ToList())
                {
                    if (entry.ExpiresUtc < now)
                    {
                        RemoveClusterSubscription(entry.SubscriptionId);
                        numExpiredSubscriptions++;
                    }
                }

                if (numExpiredSubscriptions > 0)
                {
                    log.LogInformation("Removed {numExpiredSubscriptions} expired subscriptions.", numExpiredSubscriptions);
                }
            }
        }

        private void OnMessage(object message)
        {
            switch (message)
            {
                case PayloadMessageBase payload when (payload.SourceId != senderId):
                    log.LogDebug("Received payload of type {type} from {sender}", payload.GetUntypedPayload()?.GetType(), payload.SourceId);

                    foreach (var subscriptionId in payload.SubscriptionIds)
                    {
                        if (localSubscriptions.TryGetValue(subscriptionId, out var localSubscription))
                        {
                            localSubscription.OnNext(payload.GetUntypedPayload());
                        }
                    }

                    break;

                case SubscribeMessageBase subscribe when (subscribe.SourceId != senderId):
                    log.LogDebug("Received subscription from {sender}.", subscribe.SourceId);

                    AddClusterSubscription(subscribe.SubscriptionId, subscribe.GetUntypedSubscription());
                    break;

                case UnsubscribeMessage unsubscribe when (unsubscribe.SourceId != senderId):
                    log.LogDebug("Received unsubscribe from {sender}.", unsubscribe.SourceId);

                    RemoveClusterSubscription(unsubscribe.SubscriptionId);
                    break;

                case SubscriptionsAliveMessage alive when (alive.SourceId != senderId):
                    log.LogDebug("Received alive message from {sender}.", alive.SourceId);

                    UpdateClusterSubscriptions(alive.SubscriptionIds);
                    break;

            }
        }

        public ILocalSubscription<T> Subscribe<T, TSubscription>(TSubscription subscription) where TSubscription : ISubscription, new()
        {
            return new LocalSubscription<T, TSubscription>(this, subscription);
        }

        public ILocalSubscription<T> Subscribe<T>()
        {
            return new LocalSubscription<T, Subscription<T>>(this, new Subscription<T>());
        }

        public async Task PublishAsync<T>(T message) where T : notnull
        {
            List<Guid>? remoteSubscriptionIds = null;

            foreach (var id in await evaluator.GetCandidatesAsync(message))
            {
                if (localSubscriptions.TryGetValue(id, out var localSubscription))
                {
                    localSubscription.OnNext(message);
                }
                else
                {
                    remoteSubscriptionIds ??= new List<Guid>();
                    remoteSubscriptionIds.Add(id);
                }
            }

            if (remoteSubscriptionIds == null)
            {
                return;
            }

            transport.Publish(new PayloadMessage<T>
            {
                Payload = message,
                SubscriptionIds = remoteSubscriptionIds,
                SourceId = senderId
            });
        }

        internal void SubscribeCore<TSubscription>(Guid id, IUntypedLocalSubscription localSubscription, TSubscription subscription) where TSubscription : ISubscription
        {
            localSubscriptions[id] = localSubscription;

            transport.Publish(new SubscribeMessage<TSubscription>
            {
                SubscriptionId = id,
                Subscription = subscription,
                SourceId = senderId
            });

            AddClusterSubscription(id, subscription);
        }

        internal void UnsubscribeCore(Guid id)
        {
            localSubscriptions.TryRemove(id, out _);

            transport.Publish(new UnsubscribeMessage
            {
                SubscriptionId = id,
                SourceId = senderId
            });

            RemoveClusterSubscription(id);
        }

        private void UpdateClusterSubscriptions(List<Guid> subscriptionIds)
        {
            var nextExpiration = GetNextExpiration();

            foreach (var id in subscriptionIds)
            {
                if (clusterSubscriptions.TryGetValue(id, out var entry))
                {
                    entry.UpdateExpiration(nextExpiration);
                }
            }
        }

        private void AddClusterSubscription(Guid id, ISubscription subscription)
        {
            clusterSubscriptions[id] = new SubscriptionEntry(id, subscription, GetNextExpiration());

            // The evaluator maintains a custom list to optimize the data structure for faster evaluation.
            evaluator.OnAdded(id, subscription);
        }

        private void RemoveClusterSubscription(Guid id)
        {
            if (!clusterSubscriptions.TryRemove(id, out var entry))
            {
                return;
            }

            // The evaluator maintains a custom list to optimize the data structure for faster evaluation.
            evaluator.OnRemoved(id, entry.Subscription);
        }

        private DateTime GetNextExpiration()
        {
            return Clock.UtcNow + options.SubscriptionExpirationTime;
        }
    }
}
