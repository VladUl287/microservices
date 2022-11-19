namespace BookApi.Database.Abstractions;

public abstract class EntityWithId<T> where T : struct
{
    public T Id { get; set; }
}