using GraphQLExample.Subscriptions;
using System.Collections.Concurrent;

namespace GraphQLExample.Test
{
    public sealed class TaskSubscriptionEvaluator : ISubscriptionEvaluator
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, TaskSubscription>> clusterSubscriptions = new();

        public ValueTask<IEnumerable<Guid>> GetCandidatesAsync(object message)
        {
            return new ValueTask<IEnumerable<Guid>>(GetCandidates(message));
        }

        public IEnumerable<Guid> GetCandidates(object message)
        {
            if (message is not TaskAdded taskAdded)
            {
                return Enumerable.Empty<Guid>();
            }

            if (!clusterSubscriptions.TryGetValue(taskAdded.Task.ProjectId, out var taskSubscriptions))
            {
                return Enumerable.Empty<Guid>();
            }

            return taskSubscriptions.Where(x => taskAdded.Task.Priority >= x.Value.Priority).Select(x => x.Key).ToList();
        }

        public void OnAdded(Guid id, ISubscription subscription)
        {
            if (subscription is not TaskSubscription taskSubscription)
            {
                return;
            }

            var subscriptions = clusterSubscriptions.GetOrAdd(taskSubscription.ProjectId, _ => new ConcurrentDictionary<Guid, TaskSubscription>());

            subscriptions[id] = taskSubscription;
        }

        public void OnRemoved(Guid id, ISubscription subscription)
        {
            if (subscription is not TaskSubscription taskSubscription)
            {
                return;
            }

            if (clusterSubscriptions.TryGetValue(taskSubscription.ProjectId, out var subscriptions))
            {
                subscriptions.TryRemove(id, out _);
            }
        }
    }
}
