#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace GraphQLExample.Subscriptions.Messages
{
    public abstract record MessageBase
    {
        public string SourceId { get; init; }
    }
}
