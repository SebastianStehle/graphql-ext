#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace GraphQLExample.Subscriptions.Messages
{
    public sealed record PayloadMessage<T> : PayloadMessageBase where T : notnull
    {
        public T Payload { get; init; }

        public override object GetUntypedPayload()
        {
            return Payload;
        }
    }
}
