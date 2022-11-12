using BasketApi.Database.Entities;
using BasketApi.Dtos;

namespace BasketApi.Database.Repositories.Contracts;

internal interface IBasketRepository
{
    Task<Entities.BasketItem[]> GetItemsByUser(Guid userId);

    Task<Entities.BasketItem[]> GetItemsByBook(Guid bookId);

    Task<Entities.BasketItem?> AddItem(Dtos.Item baskeItem);

    Task DeleteItem(Guid itemId);
}