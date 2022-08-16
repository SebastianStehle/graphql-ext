namespace GraphQLExample.Subscriptions
{
    public interface ISubscriptionTransport
    {
        void Publish<T>(T message);

        IDisposable Subscribe(Action<object> onMessage);
    }
}
