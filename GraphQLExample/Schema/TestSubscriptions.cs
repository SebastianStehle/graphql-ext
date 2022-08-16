using GraphQL;
using GraphQLExample.Subscriptions;
using GraphQLExample.Test;

namespace GraphQLExample.Schema
{
    public class TestSubscriptions
    {
        public static IObservable<TaskAdded> TaskAdded([FromServices] ISubscriptionService subscriptions,
            string projectId)
        {
            return subscriptions.Subscribe<TaskAdded, TaskSubscription>(new TaskSubscription
            {
                ProjectId = projectId,
                Priority = 0,
                Text = ""
            });
        }
    }
}
