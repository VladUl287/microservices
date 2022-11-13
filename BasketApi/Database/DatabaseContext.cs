using BasketApi.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BasketApi.Database;

internal sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<BasketItem> Basket { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(message =>
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        });

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Basket");

        modelBuilder.Entity<BasketItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ObjectId)
                .IsRequired();

            entity.Property(x => x.UserId)
                .IsRequired();

            entity.Property(x => x.DateAdd)
                .HasDefaultValueSql("now()")
                .IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}