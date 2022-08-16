using GraphQL;
using GraphQLExample.Test;

namespace GraphQLExample.Schema
{
    public sealed class TestQuery
    {
        public static IEnumerable<TaskItem> Tasks([FromServices] TaskService tasks,
            string projectId)
        {
            return tasks.QueryAll(projectId);
        }
    }
}
