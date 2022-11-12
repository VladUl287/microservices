using AutoMapper;
using OneOf.Types;
using BookApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using BookApi.Database.Repositories.Contracts;
using MessageBus.Contracts;
using MessageBus.Messages;

namespace BookApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
internal class BookController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IMessageBus messageBus;
	private readonly IBookRepository bookRepository;

    public BookController(IBookRepository bookRepository, IMapper mapper, IMessageBus messageBus)
    {
        this.mapper = mapper;
        this.messageBus = messageBus;
        this.bookRepository = bookRepository;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBook([FromRoute] Guid id)
    {
        var result = await bookRepository.GetBook(id);

        return result.Match<IActionResult>(
            book => Ok(book),
            notFound => NotFound());
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BookDto>))]
    public async Task<IActionResult> GetBooks([FromQuery] BookFilterDto bookFilterDto)
    {
        var books = await bookRepository.GetBooks(bookFilterDto);

        var booksDto = mapper.Map<IEnumerable<BookDto>>(books);

        return Ok(booksDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error<string>))]
    public async Task<IActionResult> CreateBook([FromBody] BookDto createBook)
	{
		var result = await bookRepository.CreateBook(createBook);

        return result.Match<IActionResult>(
            guid => CreatedAtAction(nameof(GetBook), guid, null),
            exists => BadRequest(exists));
	}

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    public async Task<IActionResult> DeleteBook([FromRoute] Guid bookId)
    {
        var result = await bookRepository.DeleteBook(bookId);

        if (result.IsT0)
        {
            await messageBus.PublishAsync(
                new DeleteItem<Guid>
                {
                    ItemId = result.AsT0.Id,
                    DateTime = DateTime.UtcNow
                });

            return NoContent();
        }

        return NotFound(bookId);
    }
}