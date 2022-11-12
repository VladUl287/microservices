using BookApi.Database.Entities;
using BookApi.Dtos;
using OneOf;
using OneOf.Types;

namespace BookApi.Database.Repositories.Contracts;

internal interface IBookRepository
{
    Task<OneOf<Book, NotFound>> GetBook(Guid id);

    Task<Book[]> GetBooks(BookFilterDto filter);

    Task<OneOf<Guid, Error<string>>> CreateBook(BookDto createBook);

    Task<OneOf<Book, NotFound>> DeleteBook(Guid id);
}