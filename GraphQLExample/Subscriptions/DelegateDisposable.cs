namespace GraphQLExample.Subscriptions
{
    public sealed class DelegateDisposable : IDisposable
    {
        private readonly Action action;

        public DelegateDisposable(Action action)
        {
            this.action = action;
        }

        public void Dispose()
        {
            try
            {
                action();
            }
            catch
            {
            }
        }
    }
}
