using System.Net;

namespace GraphQLExample.Subscriptions
{
    public sealed class HostnameSubscriptionSenderProvider : ISubscriptionSenderProvider
    {
        public string Sender => Dns.GetHostName();
    }
}
