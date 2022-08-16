
namespace GraphQLExample.Subscriptions
{
    public interface ISubscriptionService
    {
        Task PublishAsync<T>(T message) where T : notnull;

        ILocalSubscription<T> Subscribe<T, TSubscription>(TSubscription subscription) where TSubscription : ISubscription, new();
        
        ILocalSubscription<T> Subscribe<T>();
    }
}