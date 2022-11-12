using BookApi.Database.Abstractions;

namespace BookApi.Database.Entities;

internal sealed class Book : EntityWithId<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int PagesCount { get; set; }
    public DateTime? DatePublish { get; set; }
    public DateTime DateCreate { get; set; }
}