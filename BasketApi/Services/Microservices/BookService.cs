using BasketApi.Services.Contracts;

namespace BasketApi.Services.Microservices;

public sealed class BookService : IBookService
{
    private readonly HttpClient httpClient;

    public BookService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<object?> GetBook(Guid id)
    {
        return await httpClient.GetFromJsonAsync<object?>($"/api/book/getBook/{id}");
    }
}