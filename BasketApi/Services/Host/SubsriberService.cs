using BasketApi.Database.Repositories;
using BasketApi.Database.Repositories.Contracts;
using MessageBus.Contracts;
using MessageBus.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace BasketApi.Services.Host;

public class SubsriberService : IHostedService
{
    private readonly IMessageBus messageBus;
    private readonly IServiceProvider serviceProvider;

    public SubsriberService(IMessageBus messageBus, IServiceProvider serviceProvider)
    {
        this.messageBus = messageBus;
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await messageBus.SubscribeAsync<DeleteItem<Guid>>("book_deletion", Handler, cancellationToken);
    }

    public async Task Handler(DeleteItem<Guid> item)
    {
        using var scope = serviceProvider.CreateAsyncScope();
        var basketRepository = scope.ServiceProvider.GetService<IBasketRepository>();

        if (basketRepository is null)
        {
            throw new Exception();
        }

        await basketRepository.DeleteItem(item.ItemId);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        messageBus.Dispose();

        return Task.CompletedTask;
    }
}
