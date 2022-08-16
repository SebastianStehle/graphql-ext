#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace GraphQLExample.Subscriptions.Messages
{
    public sealed record SubscriptionsAliveMessage : MessageBase
    {
        public List<Guid> SubscriptionIds { get; init; }
    }
}
