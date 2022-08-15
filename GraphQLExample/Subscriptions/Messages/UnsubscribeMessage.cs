namespace GraphQLExample.Subscriptions.Messages
{
    public sealed record UnsubscribeMessage
    {
        public Guid SubscriptionId { get; init; }
    }
}
