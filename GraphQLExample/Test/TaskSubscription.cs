using GraphQLExample.Subscriptions;

namespace GraphQLExample.Test
{
    public sealed class TaskSubscription : ISubscription
    {
        public string ProjectId { get; set; } = default!;

        public string? Text { get; set; }

        public int Priority { get; set; }

        public ValueTask<bool> ShouldHandle(object message)
        {
            return default;
        }
    }
}
