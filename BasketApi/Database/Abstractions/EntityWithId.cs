namespace BasketApi.Database.Abstractions;

internal abstract class EntityWithId<T> where T : struct
{
    public T Id { get; set; }
}