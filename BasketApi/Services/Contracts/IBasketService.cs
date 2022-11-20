using BasketApi.Dtos;

namespace BasketApi.Services.Contracts;

public interface IBasketService
{
    Task<ItemDto[]> GetItemsByUser(Guid userId);

    Task<ItemDto> AddItem(ItemDto baskeItem);

    Task DeleteItem(Guid itemId);

    Task DeleteItemsByObject(Guid objectId);
}