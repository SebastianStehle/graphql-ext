using GraphQLExample.Subscriptions.Messages;

namespace GraphQLExample.Subscriptions
{
    public sealed class InMemoryTransport : ISubscriptionTransport
    {
        private readonly List<Action<MessageBase>> handlers = new();

        public void Publish<TPayload>(PayloadMessage<TPayload> message) where TPayload : notnull
        {
            PublishCore(message);
        }

        public void Publish<TSubscription>(SubscribeMessage<TSubscription> message) where TSubscription : ISubscription
        {
            PublishCore(message);
        }

        public void Publish(SubscriptionsAliveMessage message)
        {
            PublishCore(message);
        }

        public void Publish(UnsubscribeMessage message)
        {
            PublishCore(message);
        }

        private void PublishCore(MessageBase message)
        {
            foreach (var handler in handlers)
            {
                handler(message);
            }
        }

        public IDisposable Subscribe(Action<MessageBase> onMessage)
        {
            handlers.Add(onMessage);

            return new DelegateDisposable(() =>
            {
                handlers.Remove(onMessage);
            });
        }
    }
}
