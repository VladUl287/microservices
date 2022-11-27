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
        var result = await httpClient.GetAsync($"/api/book/getBook/{id}");

        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return result.Content.ReadFromJsonAsync<object?>();
        }

        return null;
    }
}