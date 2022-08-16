#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace GraphQLExample.Subscriptions.Messages
{
    public abstract record PayloadMessageBase : MessageBase
    {
        public List<Guid> SubscriptionIds { get; init; }

        // This is a method, so it does not get serialized.
        public abstract object? GetUntypedPayload();
    }
}
