namespace MessageBus.Messages;

public sealed class DeleteItem<T> where T : struct
{
    public required T ObjectId { get; init; }

    public required DateTime DateTime { get; init; }
}