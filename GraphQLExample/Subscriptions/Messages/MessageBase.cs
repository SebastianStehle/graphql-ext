namespace GraphQLExample.Subscriptions.Messages
{
    public abstract record MessageBase
    {
        public string SourceId { get; init; }
    }
}
