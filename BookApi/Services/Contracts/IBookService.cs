using OneOf;
using OneOf.Types;
using BookApi.Dtos;
using BookApi.Database.Entities;

namespace BookApi.Services.Contracts;

public interface IBookService
{
    Task<OneOf<BookDto, NotFound>> GetBook(Guid id);

    Task<BookDto[]> GetBooks(BookFilterDto filter);

    Task<OneOf<Guid, Error<string>>> CreateBook(BookDto createBook);

    Task<OneOf<Guid, NotFound>> DeleteBook(Guid id);
}
