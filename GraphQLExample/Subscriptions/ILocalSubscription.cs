namespace GraphQLExample.Subscriptions
{
    public interface ISubscription<T> : IObservable<T>
    {
    }
}
