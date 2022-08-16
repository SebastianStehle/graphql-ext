
namespace GraphQLExample.Subscriptions
{
    public interface ISubscription
    {
        ValueTask<bool> ShouldHandle(object message);
    }
}