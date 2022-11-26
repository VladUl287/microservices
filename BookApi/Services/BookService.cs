using AutoMapper;
using BookApi.Common;
using BookApi.Database;
using BookApi.Database.Entities;
using BookApi.Dtos;
using BookApi.Services.Contracts;
using MessageBus.Contracts;
using MessageBus.Messages;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace BookApi.Services;

public class BookService : IBookService
{
    private readonly IMapper mapper;
    private readonly IMessageBus messageBus;
    private readonly DatabaseContext dbContext;

    public BookService(DatabaseContext dbContext, IMessageBus messageBus, IMapper mapper)
    {
        this.mapper = mapper;
        this.dbContext = dbContext;
        this.messageBus = messageBus;
    }

    public async Task<OneOf<BookDto, NotFound>> GetBook(Guid id)
    {
        var result = await dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (result is null)
        {
            return new NotFound();
        }

        return mapper.Map<BookDto>(result); ;
    }

    public async Task<BookDto[]> GetBooks(BookFilterDto filter)
    {
        var query = dbContext.Books.AsNoTracking();

        if (filter.Page.HasValue && filter.Size.HasValue)
        {
            var skip = (filter.Page.Value - 1) * filter.Size.Value;
            query = query.Skip(skip).Take(filter.Size.Value);
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            var name = filter.Name.ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(name));
        }

        var result = await query.ToArrayAsync();

        return mapper.Map<BookDto[]>(result);
    }

    public async Task<OneOf<Guid, Error<string>>> CreateBook(BookDto createBook)
    {
        var exists = await dbContext.Books
            .AnyAsync(x => x.Name == createBook.Name);

        if (exists)
        {
            return new Error<string>(ErrorsDescription.BookAlreadyExists);
        }

        var book = new Book()
        {
            Name = createBook.Name,
        };

        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        return book.Id;
    }

    public async Task<OneOf<Guid, NotFound>> DeleteBook(Guid id)
    {
        var book = await dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (book is null)
        {
            return new NotFound();
        }

        dbContext.Books.Remove(book);
        await dbContext.SaveChangesAsync();

        await messageBus.PublishAsync(
            new DeleteItem<Guid>
            {
                ObjectId = book.Id,
                DateTime = DateTime.UtcNow
            });

        return book.Id;
    }
}
