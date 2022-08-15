namespace GraphQLExample.Subscriptions
{
    internal sealed class Subscription<T> : ISubscription<T>, IUntypedSubscription, IDisposable
    {
        private readonly SubscriptionService subscriptionService;
        private readonly Func<T, bool> filter;
        private IObserver<T>? currentObserver;

        public Guid Id { get; } = Guid.NewGuid();

        public Dictionary<string, string> Context { get; }

        public Subscription(SubscriptionService subscriptionService, Dictionary<string, string> context)
        {
            this.subscriptionService = subscriptionService;
            this.Context = context;
        }

        private void SubscribeCore(IObserver<T> observer)
        {
            if (currentObserver == null)
            {
                throw new InvalidOperationException("Can only have one observer.");
            }

            subscriptionService.SubscribeCore(this);

            currentObserver = observer;
        }

        void IDisposable.Dispose()
        {
            if (currentObserver == null)
            {
                return;
            }

            subscriptionService.UnsubscribeCore(this);

            currentObserver = null;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            SubscribeCore(observer);

            return this;
        }

        public void OnError(Exception error)
        {
            if (error != null)
            {
                currentObserver?.OnError(error);
            }
        }

        public void OnNext(object value)
        {
            if (value is T typed && filter(typed))
            {
                currentObserver?.OnNext(typed);
            }
        }
    }
}
