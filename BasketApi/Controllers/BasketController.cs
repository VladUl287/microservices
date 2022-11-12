using AutoMapper;
using BasketApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using BasketApi.Database.Repositories.Contracts;
using BasketApi.Common;
using BasketApi.Services.Contracts;

namespace BasketApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
internal class BasketController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IBasketRepository basketRepository;
    private readonly IBookService catalogService;

    public BasketController(IBasketRepository basketRepository, IBookService catalogService, IMapper mapper)
    {
        this.mapper = mapper;
        this.basketRepository = basketRepository;
        this.catalogService = catalogService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Item>))]
    public async Task<IActionResult> GetItems([FromHeader] Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest(userId);
        }

        var items = await basketRepository.GetItemsByUser(userId);

        var itemsDtos = mapper.Map<IEnumerable<Item>>(items);

        return Ok(itemsDtos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> AddItem([FromBody] Item basketItem)
    {
        var book = await catalogService.GetBook(basketItem.BookId);

        if (book is null)
        {
            return BadRequest(Errors.ItemNotExists);
        }

        var item = await basketRepository.AddItem(basketItem);

        if (item is null)
        {
            return BadRequest(Errors.ItemAlreadyExists);
        }

        return Created(nameof(AddItem), item);
    }

    [HttpDelete("{itemId:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteItem([FromRoute] Guid itemId)
    {
        if (itemId == Guid.Empty)
        {
            return BadRequest(itemId);
        }

        await basketRepository.DeleteItem(itemId);

        return NoContent();
    }
}