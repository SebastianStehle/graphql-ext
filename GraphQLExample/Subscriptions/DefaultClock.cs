namespace GraphQLExample.Subscriptions
{
    public sealed class DefaultClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
