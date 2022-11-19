using BasketApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using BasketApi.Common;
using BasketApi.Services.Contracts;

namespace BasketApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService basketService;

    public BasketController(IBasketService basketService)
    {
        this.basketService = basketService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ItemDto>))]
    public async Task<IActionResult> GetItems([FromHeader] Guid userId)
    {
        return Ok(await basketService.GetItemsByUser(userId));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> AddItem([FromBody] ItemDto basketItem)
    {
        var item = await basketService.AddItem(basketItem);

        return Created(nameof(AddItem), item);
    }

    [HttpDelete("{itemId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid itemId)
    {
        await basketService.DeleteItem(itemId);

        return NoContent();
    }
}