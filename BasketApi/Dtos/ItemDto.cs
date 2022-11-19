namespace BasketApi.Dtos;

public sealed class ItemDto
{
    public required Guid UserId { get; init; }

    public required Guid ObjectId { get; init; }

    public required DateTime Date { get; init; }
}