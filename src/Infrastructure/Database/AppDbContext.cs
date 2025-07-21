using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
{
    public DbSet<Key> Keys { get; set; }

    public DbSet<SearchHistory> SearchHistories { get; set; }
}