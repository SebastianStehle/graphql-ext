namespace GraphQLExample.Schema
{
    public sealed class TaskInput
    {
        public string? ProjectId { get; set; }

        public string? Text { get; set; }

        public string? Title { get; set; }

        public int Priority { get; set; }
    }
}
