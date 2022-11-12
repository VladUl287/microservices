using BasketApi.Database.Entities;
using BasketApi.Database.Repositories.Contracts;
using BasketApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BasketApi.Database.Repositories;

internal sealed class BasketRepository : IBasketRepository
{
    private readonly DatabaseContext dbContext;

    public BasketRepository(DatabaseContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Entities.BasketItem[]> GetItemsByUser(Guid userId)
    {
        return await dbContext.Basket
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<Entities.BasketItem[]> GetItemsByBook(Guid bookId)
    {
        return await dbContext.Basket
            .Where(x => x.ObjectId == bookId)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<Entities.BasketItem?> AddItem(Dtos.Item baskeItem)
    {
        var exists = await dbContext.Basket.AnyAsync(x => x.UserId == baskeItem.UserId && x.ObjectId == baskeItem.BookId);

        if (exists)
        {
            return null;
        }

        var item = new Entities.BasketItem()
        {
            UserId = baskeItem.UserId,
            ObjectId = baskeItem.BookId,
            DateAdd = DateTime.UtcNow
        };

        await dbContext.Basket.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return item;
    }

    public async Task DeleteItem(Guid itemId)
    {
        var item = await dbContext.Basket
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == itemId);

        if (item is null)
        {
            return;
        }

        dbContext.Basket.Remove(item);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteItems(Entities.BasketItem[] basketItems)
    {
        if (basketItems is null || basketItems.Length == 0)
        {
            return;
        }

        dbContext.Basket.RemoveRange(basketItems);
        await dbContext.SaveChangesAsync();
    }
}
