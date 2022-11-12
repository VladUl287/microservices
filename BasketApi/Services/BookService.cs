namespace BasketApi.Services;

internal sealed class BookService
{
	private readonly HttpClient httpClient;

	public BookService(HttpClient httpClient)
	{
		this.httpClient = httpClient;
	}

	public async Task<object?> GetBookAsync(Guid id)
	{
		return await httpClient.GetFromJsonAsync<object?>($"/api/catalog/getBook/{id}");
	}
}