using GraphQLExample.Subscriptions.Messages;

namespace GraphQLExample.Subscriptions
{
    public interface ISubscriptionTransport
    {
        void Publish<TPayload>(PayloadMessage<TPayload> message) where TPayload : notnull;

        void Publish<TSubscription>(SubscribeMessage<TSubscription> message) where TSubscription : ISubscription;

        void Publish(SubscriptionsAliveMessage message);

        void Publish(UnsubscribeMessage message);

        IDisposable Subscribe(Action<MessageBase> onMessage);
    }
}
