namespace GraphQLExample.Subscriptions.Messages
{
    public abstract record SubscribeMessageBase : MessageBase
    {
        public Guid SubscriptionId { get; init; }

        // This is a method, so it does not get serialized.
        public abstract ISubscription GetUntypedSubscription();
    }
}