using GraphQL;
using GraphQL.DI;

namespace GraphQLExample.Schema
{
    public class Configure : IConfigureExecution
    {
        public float SortOrder => 0;

        public Task<ExecutionResult> ExecuteAsync(ExecutionOptions options, ExecutionDelegate next)
        {
            options.UnhandledExceptionDelegate = x =>
            {
                return Task.CompletedTask;
            };

            return next(options);
        }
    }
}
