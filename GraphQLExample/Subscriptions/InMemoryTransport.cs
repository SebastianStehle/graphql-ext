namespace GraphQLExample.Subscriptions
{
    public sealed class InMemoryTransport : ISubscriptionTransport
    {
        private readonly List<Action<object>> handlers = new List<Action<object>>();

        public void Publish<T>(T message)
        {
            foreach (var handler in handlers)
            {
                handler(message!);
            }
        }

        public IDisposable Subscribe(Action<object> onMessage)
        {
            handlers.Add(onMessage);

            return new DelegateDisposable(() =>
            {
                handlers.Remove(onMessage);
            });
        }
    }
}
