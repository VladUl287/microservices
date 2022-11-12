using BasketApi.Database.Abstractions;

namespace BasketApi.Database.Entities;

internal sealed class BasketItem : EntityWithId<Guid>
{
    public Guid ObjectId { get; set; }
    public Guid UserId { get; set; }
    public DateTime DateAdd { get; set; }
}