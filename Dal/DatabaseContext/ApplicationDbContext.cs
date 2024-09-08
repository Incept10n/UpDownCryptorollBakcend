using Dal.Entities;
using Dal.Enums;
using Microsoft.EntityFrameworkCore;

namespace Dal.DatabaseContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Price> Prices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Price>().HasData(
            new Price { Id = 1, Coin = Coin.Btc, Value = 0 },
            new Price { Id = 2, Coin = Coin.Eth, Value = 0 },
            new Price { Id = 3, Coin = Coin.Ton, Value = 0 });
    }
}