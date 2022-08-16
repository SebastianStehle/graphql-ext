namespace GraphQLExample.Subscriptions
{
    public interface IUntypedLocalSubscription
    {
        void OnError(Exception exception);

        void OnNext(object? value);
    }
}
