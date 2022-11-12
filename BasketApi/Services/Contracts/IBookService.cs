namespace BasketApi.Services.Contracts;

internal interface IBookService
{
    Task<object?> GetBook(Guid id);
}