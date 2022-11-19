using BasketApi.Services.Contracts;
using MessageBus.Contracts;
using MessageBus.Messages;

namespace BasketApi.Services.Host;

public class SubsriberService : IHostedService
{
    private readonly IMessageBus messageBus;
    private readonly IServiceProvider serviceProvider;

    private const string SubscriptionId = "book_deletion";

    public SubsriberService(IMessageBus messageBus, IServiceProvider serviceProvider)
    {
        this.messageBus = messageBus;
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await messageBus.SubscribeAsync<DeleteItem<Guid>>(SubscriptionId, Handler, cancellationToken);
    }

    private async Task Handler(DeleteItem<Guid> item)
    {
        using var scope = serviceProvider.CreateAsyncScope();
        var basketRepository = scope.ServiceProvider.GetService<IBasketService>();

        if (basketRepository is null)
        {
            throw new NullReferenceException(nameof(basketRepository));
        }

        await basketRepository.DeleteItemsByObject(item.ObjectId);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        messageBus.Dispose();

        return Task.CompletedTask;
    }
}
