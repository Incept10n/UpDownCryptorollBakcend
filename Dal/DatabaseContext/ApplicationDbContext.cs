using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.DatabaseContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Match> Matches { get; set; }
}