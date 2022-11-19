namespace BasketApi.Services.Contracts;

public interface IBookService
{
    Task<object?> GetBook(Guid id);
}