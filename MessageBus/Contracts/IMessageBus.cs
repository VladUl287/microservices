namespace MessageBus.Contracts;

public interface IMessageBus : IDisposable
{
    Task PublishAsync<T>(T message);

    Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> handler, CancellationToken token);
}