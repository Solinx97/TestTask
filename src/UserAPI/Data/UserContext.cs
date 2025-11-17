using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Data;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<JournalModel>? Journal { get; }

    public DbSet<NodeModel>? Node { get; }

    public DbSet<UserModel>? User { get; }
}