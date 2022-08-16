namespace GraphQLExample.Subscriptions
{
    public interface ISubscriptionEvaluator
    {
        ValueTask<IEnumerable<Guid>> GetCandidatesAsync(object message);

        void OnAdded(Guid id, ISubscription subscription);

        void OnRemoved(Guid id);
    }
}
