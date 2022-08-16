using GraphQLExample.Subscriptions;

namespace GraphQLExample.Test
{
    public sealed class TaskSubscription : ISubscription
    {
        public string? ProjectId { get; set; }

        public string? Text { get; set; }

        public int Priority { get; set; }

        public ValueTask<bool> ShouldHandle(object message)
        {
            var shouldHandle = message is TaskAdded added && 
                added.TaskItem?.ProjectId == ProjectId && 
                added.TaskItem?.Priority >= Priority;

            return new ValueTask<bool>(shouldHandle);
        }
    }
}
