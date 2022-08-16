namespace GraphQLExample.Subscriptions.Messages
{
    public sealed record SubscriptionsAliveMessage : MessageBase
    {
        public List<Guid> SubscriptionIds { get; init; } = null!;
    }
}
