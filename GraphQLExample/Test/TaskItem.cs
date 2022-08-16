namespace GraphQLExample.Test
{
    public sealed class TaskItem
    {
        public string ProjectId { get; set; } = default!;

        public string Text { get; set; } = default!;

        public int Priority { get; set; }
    }
}
