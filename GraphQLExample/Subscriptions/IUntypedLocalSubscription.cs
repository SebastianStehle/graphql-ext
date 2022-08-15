namespace GraphQLExample.Subscriptions
{
    public interface IUntypedSubscription
    {
        Guid Id { get; }

        Dictionary<string, string> Context { get; }

        void OnError(Exception exception);

        void OnNext(object value);
    }
}
