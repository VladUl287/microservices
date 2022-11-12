using BasketApi.Database.Repositories.Contracts;
using MessageBus.Contracts;
using MessageBus.Messages;

namespace BasketApi.Services;

internal class SubsriberService : IHostedService
{
    private readonly IMessageBus messageBus;
    private readonly IBasketRepository basketRepository;

    public SubsriberService(IMessageBus messageBus, IBasketRepository basketRepository)
    {
        this.messageBus = messageBus;
        this.basketRepository = basketRepository;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        async Task handler(DeleteItem<Guid> item)
        {
            await basketRepository.DeleteItem(item.ItemId);
        }

        await messageBus.SubscribeAsync<DeleteItem<Guid>>("book_deletion", handler, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        messageBus.Dispose();

        return Task.CompletedTask;
    }
}
