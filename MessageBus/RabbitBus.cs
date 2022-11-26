using EasyNetQ;
using MessageBus.Contracts;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace MessageBus;

public sealed class RabbitBus : IMessageBus
{
    private readonly IBus bus;
    private readonly IAsyncPolicy retryPolicy;

    private bool disposed;

    public RabbitBus(string connection)
    {
        bus = RabbitHutch.CreateBus(connection);
        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));
    }

    public List<SubscriptionResult> SubscriptionResults { get; init; } = new();

    public async Task PublishAsync<T>(T message)
    {
        await retryPolicy.ExecuteAsync(() => bus.PubSub.PublishAsync(message));
    }

    public async Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> handler, CancellationToken token = default)
    {
        var subscriptionResult = await bus.PubSub.SubscribeAsync(subscriptionId, handler, token);

        SubscriptionResults.Add(subscriptionResult);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            for (int i = 0; i < SubscriptionResults.Count; i++)
            {
                SubscriptionResults[i].Dispose();
            }

            bus?.Dispose();
        }

        disposed = true;
    }
}