namespace GraphQLExample.Subscriptions
{
    public sealed class SubscriptionOptions
    {
        public TimeSpan SubscriptionExpirationTime { get; set; } = TimeSpan.FromMinutes(30);

        public TimeSpan SubscriptionUpdateTime { get; set; } = TimeSpan.FromMinutes(5);
    }
}
