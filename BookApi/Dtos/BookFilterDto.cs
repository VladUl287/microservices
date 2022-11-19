namespace BookApi.Dtos;

public sealed class BookFilterDto
{
    public int? Page { get; init; }

    public int? Size { get; init; }

    public string Name { get; init; } = string.Empty;
}