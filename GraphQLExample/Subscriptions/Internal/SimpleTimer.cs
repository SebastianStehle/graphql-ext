using Microsoft.Extensions.Logging;

namespace GraphQLExample.Subscriptions.Internal
{
    internal sealed class SimpleTimer : IAsyncDisposable
    {
        private readonly CancellationTokenSource stopToken = new CancellationTokenSource();

        public bool IsDisposed => stopToken.IsCancellationRequested;

        public SimpleTimer(Func<CancellationToken, Task> action, TimeSpan interval, ILogger log)
        {
            Task.Run(async () =>
            {
                try
                {
                    while (!stopToken.IsCancellationRequested)
                    {
                        try
                        {
                            await action(stopToken.Token);

                            await Task.Delay(interval, stopToken.Token);
                        }
                        catch (OperationCanceledException)
                        {
                        }
                        catch (Exception ex)
                        {
                            log.LogWarning(ex, "Failed to execute timer.");
                        }
                    }
                }
                catch
                {
                    return;
                }
            }, stopToken.Token);
        }

        public ValueTask DisposeAsync()
        {
            stopToken.Cancel();

            return default;
        }
    }
}
