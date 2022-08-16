namespace GraphQLExample.Subscriptions
{
    public class Subscription<T> : ISubscription
    {
        public virtual ValueTask<bool> ShouldHandle(object message)
        {
            return new ValueTask<bool>(message is T);
        }
    }
}
