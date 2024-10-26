using Dal.Entities;
using Dal.Entities.User;
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
    
    public DbSet<UserTask> UserTasks { get; set; }
    public DbSet<RewardTask> RewardTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Price>().HasData(
            new Price { Id = 1, Coin = Coin.Btc, Value = 0 },
            new Price { Id = 2, Coin = Coin.Eth, Value = 0 },
            new Price { Id = 3, Coin = Coin.Ton, Value = 0 });

        modelBuilder.Entity<RewardTask>().HasData(
            new RewardTask {Id = 1, Name = "Subscribe to telegram", Reward = 1500},
            new RewardTask {Id = 2, Name = "Subscribe to X", Reward = 1500},
            new RewardTask {Id = 3, Name = "Subscribe to VK", Reward = 1500},
            new RewardTask {Id = 4, Name = "Invite a friend", Reward = 1000},
            new RewardTask {Id = 5, Name = "Read whitepaper", Reward = 1000},
            new RewardTask {Id = 6, Name = "Complete a quiz", Reward = 3900}
        );
    }
}