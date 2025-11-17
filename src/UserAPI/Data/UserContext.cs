using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Data;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<JournalInfoModel>? JournalInfo { get; }

    public DbSet<JournalModel>? Journal { get; }

    public DbSet<NodeModel>? Node { get; }
}