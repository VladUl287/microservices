using BasketApi.Dtos;
using BasketApi.Database.Entities;

namespace BasketApi.Services.Contracts;

public interface IBasketService
{
    Task<ItemDto[]> GetItemsByUser(Guid userId);

    Task<ItemDto> AddItem(ItemDto baskeItem);

    Task DeleteItem(Guid itemId);
}