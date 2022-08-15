namespace GraphQLExample.Subscriptions
{
    public interface ISubscriptionTransport
    {
        void Publish(object message);
    }
}
