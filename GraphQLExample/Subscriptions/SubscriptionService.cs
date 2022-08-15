using GraphQLExample.Subscriptions.Messages;
using System.Collections.Concurrent;

namespace GraphQLExample.Subscriptions
{
    public sealed class SubscriptionService : ISubscriptionService
    {
        private readonly ConcurrentDictionary<Guid, IUntypedLocalSubscription> localSubscriptions = new();
        private readonly ISubscriptionTransport transport;

        public SubscriptionService(ISubscriptionTransport transport)
        {
            this.transport = transport;
        }

        public ILocalSubscription<T> Subscribe<T>()
        {
            return new LocalSubscription<T>(this, x => true);
        }

        public ILocalSubscription<T> Subscribe<T>(Func<T, bool> filter)
        {
            return new Subscription<T>(this);
        }

        public void Publish<T>(Guid subscriptionId, T message) where T : notnull
        {
            if (localSubscriptions.TryGetValue(subscriptionId, out var localSubscription))
            {
                localSubscription.OnNext(message);
                return;
            }

            transport.Publish(new PayloadMessage<T>
            {
                Payload = message
            });
        }

        internal void SubscribeCore(IUntypedLocalSubscription subscription)
        {
            transport.Publish(new SubscribeMessage
            {
                SubscriptionId = subscription.Id
            });
        }

        internal void UnsubscribeCore(IUntypedLocalSubscription subscription)
        {
            transport.Publish(new UnsubscribeMessage
            {
                SubscriptionId = subscription.Id
            });
        }
    }
}
