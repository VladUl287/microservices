namespace BasketApi.Dtos;

internal sealed class Item
{
    public Guid UserId { get; init; }

    public Guid BookId { get; init; }

    public DateTime Date { get; init; }
}