namespace GraphQLExample.Subscriptions.Messages
{
    public sealed record PayloadMessage<T> : PayloadMessageBase where T : notnull
    {
        public T Payload { get; init; } = default!;

        public override object GetUntypedPayload()
        {
            return Payload;
        }
    }
}
