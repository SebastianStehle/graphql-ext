#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace GraphQLExample.Subscriptions.Messages
{
    public sealed record SubscribeMessage<T> : SubscribeMessageBase where T : ISubscription
    {
        public T Subscription { get; init; }

        public override ISubscription GetUntypedSubscription()
        {
            return Subscription;
        }
    }
}
