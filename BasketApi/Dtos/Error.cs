namespace BasketApi.Dtos;

internal sealed class Error
{
	public Error(string value)
	{
		Value = value;
	}

	public string Value { get; }
}