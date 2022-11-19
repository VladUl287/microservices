using System.Diagnostics;
using BookApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Database;

public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> contextOptions) : base(contextOptions)
    {;
    }

    public DbSet<Book> Books { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(message =>
        {
            Debug.WriteLine(message);
            Console.WriteLine(message);
        });

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Book");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(x => x.Name)
                .IsUnique();

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(10000);

            entity.Property(x => x.ImageUrl)
                .IsRequired(false);

            entity.Property(x => x.PagesCount)
                .IsRequired();

            entity.Property(x => x.DatePublish)
                .IsRequired(false);

            entity.Property(x => x.DateCreate)
                .IsRequired()
                .HasDefaultValueSql("now()");
        });

        base.OnModelCreating(modelBuilder);
    }
}
