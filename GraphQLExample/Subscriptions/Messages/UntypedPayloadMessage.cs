namespace GraphQLExample.Subscriptions.Messages
{
    public abstract record UntypedPayloadMessage
    {
        public abstract object GetUntypedPayload();
    }
}
