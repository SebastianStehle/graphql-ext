namespace GraphQLExample.Schema
{
    public sealed class TaskInput
    {
        public string ProjectId { get; set; } = null!;

        public string Text { get; set; } = null!;

        public int Priority { get; set; }
    }
}
