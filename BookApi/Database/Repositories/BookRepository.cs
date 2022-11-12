using BookApi.Dtos;
using BookApi.Database.Entities;
using BookApi.Database.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using BookApi.Common;

namespace BookApi.Database.Repositories;

internal sealed class BookRepository : IBookRepository
{
    private readonly DatabaseContext DatabaseContext;

    public BookRepository(DatabaseContext context)
    {
        DatabaseContext = context;
    }

    public async Task<OneOf<Book, NotFound>> GetBook(Guid id)
    {
        var result = await DatabaseContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (result is null)
        {
            return new NotFound();
        }

        return result;
    }

    public async Task<Book[]> GetBooks(BookFilterDto filter)
    {
        var query = DatabaseContext.Books.AsNoTracking();

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

        return await query.ToArrayAsync();
    }

    public async Task<OneOf<Guid, Error<string>>> CreateBook(BookDto createBook)
    {
        var exists = await DatabaseContext.Books.AnyAsync(x => x.Name == createBook.Name);

        if (exists)
        {
            return new Error<string>(Errors.BookAlreadyExists);
        }

        var book = new Book()
        {
            Name = createBook.Name,
        };

        await DatabaseContext.Books.AddAsync(book);
        await DatabaseContext.SaveChangesAsync();

        return book.Id;
    }

    public async Task<OneOf<Book, NotFound>> DeleteBook(Guid id)
    {
        var book = await DatabaseContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (book is null)
        {
            return new NotFound();
        }

        await DatabaseContext.Books
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
        await DatabaseContext.SaveChangesAsync();

        return book;
    }
}