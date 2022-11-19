using AutoMapper;
using BasketApi.Dtos;
using BasketApi.Database;
using BasketApi.Database.Entities;
using BasketApi.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using BasketApi.Common;

namespace BasketApi.Services.Microservices;

public sealed class BasketService : IBasketService
{
    private readonly IMapper mapper;
    private readonly IBookService bookService;
    private readonly DatabaseContext dbContext;

    public BasketService(DatabaseContext dbContext, IMapper mapper, IBookService bookService)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.bookService = bookService;
    }

    public async Task<ItemDto[]> GetItemsByUser(Guid userId)
    {
        var basketItems = await dbContext.Basket
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .ToArrayAsync();

        return mapper.Map<ItemDto[]>(basketItems);
    }

    public async Task<ItemDto> AddItem(ItemDto baskeItem)
    {
        var book = await bookService.GetBook(baskeItem.ObjectId);

        if (book is null)
        {
           throw new ArgumentException(ErrorsDescription.ItemNotExists);
        }

        var exists = await dbContext.Basket
            .AnyAsync(x => x.UserId == baskeItem.UserId && x.ObjectId == baskeItem.ObjectId);

        if (exists)
        {
            throw new ArgumentException(ErrorsDescription.ItemAlreadyExists);
        }

        var item = new BasketItem()
        {
            UserId = baskeItem.UserId,
            ObjectId = baskeItem.ObjectId,
            DateAdd = DateTime.UtcNow
        };

        await dbContext.Basket.AddAsync(item);
        await dbContext.SaveChangesAsync();

        return baskeItem;
    }

    public async Task DeleteItem(Guid itemId)
    {
        await dbContext.Basket
            .Where(x => x.Id == itemId)
            .ExecuteDeleteAsync();

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteItemsByObject(Guid objectId)
    {
        await dbContext.Basket
            .Where(x => x.ObjectId == objectId)
            .ExecuteDeleteAsync();

        await dbContext.SaveChangesAsync();
    }
}