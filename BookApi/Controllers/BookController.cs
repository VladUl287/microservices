using OneOf.Types;
using BookApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using BookApi.Services.Contracts;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BookController : ControllerBase
{
	private readonly IBookService bookService;

    public BookController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBook([FromRoute] Guid id)
    {
        var result = await bookService.GetBook(id);

        return result.Match<IActionResult>(
            book => Ok(book),
            notFound => NotFound());
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookDto>))]
    public async Task<IActionResult> GetBooks([FromQuery] BookFilterDto bookFilterDto)
    {
        return Ok(await bookService.GetBooks(bookFilterDto));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error<string>))]
    public async Task<IActionResult> CreateBook([FromBody] BookDto createBook)
    {
        var result = await bookService.CreateBook(createBook);

        return result.Match<IActionResult>(
            guid => CreatedAtAction(nameof(GetBook), guid, null),
            error => BadRequest(error));
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
    {
        var result = await bookService.DeleteBook(id);

        return result.Match<IActionResult>(
            success => NoContent(),
            notFound => NotFound(id));
    }
}