#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace GraphQLExample.Test
{
    public sealed class TaskAdded
    {
        public TaskItem Task { get; set; }
    }
}
