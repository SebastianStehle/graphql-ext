namespace GraphQLExample.Subscriptions.Messages
{
    public sealed record SubscribeMessage<T> : SubscribeMessageBase where T : ISubscription
    {
        public T Subscription { get; init; } = default!;

        public override ISubscription GetUntypedSubscription()
        {
            return Subscription;
        }
    }
}
