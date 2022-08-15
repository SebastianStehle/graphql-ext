namespace GraphQLExample.Subscriptions.Messages
{
    public sealed class SubscribeMessage
    {
        public Guid SubscriptionId { get; init; }

        public string EventType { get; init; }
    }
}
