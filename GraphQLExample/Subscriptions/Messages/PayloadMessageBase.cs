namespace GraphQLExample.Subscriptions.Messages
{
    public abstract record PayloadMessageBase : MessageBase
    {
        public List<Guid> SubscriptionIds { get; init; } = default!;

        // This is a method, so it does not get serialized.
        public abstract object? GetUntypedPayload();
    }
}
