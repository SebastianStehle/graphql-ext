namespace GraphQLExample.Subscriptions.Messages
{
    public sealed record UnsubscribeMessage : MessageBase
    {
        public Guid SubscriptionId { get; init; }
    }
}
