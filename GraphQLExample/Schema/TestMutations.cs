using GraphQL;
using GraphQLExample.Test;

namespace GraphQLExample.Schema
{
    public sealed class TestMutations
    {
        public static TaskItem CreateTask([FromServices] TaskService tasks,
            TaskInput input)
        {
            var taskItem = new TaskItem
            {
                ProjectId = input.ProjectId,
                Priority = input.Priority,
                Text = input.Text
            };

            tasks.Add(taskItem);

            return taskItem;
        }
    }
}
