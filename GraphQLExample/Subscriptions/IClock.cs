namespace GraphQLExample.Subscriptions
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
