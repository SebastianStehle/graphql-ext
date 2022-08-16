using System.Collections.Concurrent;

namespace GraphQLExample.Subscriptions
{
    public sealed class DefaultSubscriptionEvaluator : ISubscriptionEvaluator
    {
        private readonly ConcurrentDictionary<Guid, ISubscription> clusterSubscriptions = new();

        public async ValueTask<IEnumerable<Guid>> GetCandidatesAsync(object message)
        {
            List<Guid>? result = null;

            foreach (var (id, subscription) in clusterSubscriptions)
            {
                if (await subscription.ShouldHandle(message))
                {
                    result ??= new List<Guid>();
                    result.Add(id);
                }
            }

            return result ?? Enumerable.Empty<Guid>();
        }

        public void OnAdded(Guid id, ISubscription subscription)
        {
            clusterSubscriptions[id] = subscription;
        }

        public void OnRemoved(Guid id, ISubscription subscription)
        {
            clusterSubscriptions.TryRemove(id, out _);
        }
    }
}
