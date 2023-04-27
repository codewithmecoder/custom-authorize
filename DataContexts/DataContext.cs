using CustomAuthorize.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomAuthorize.DataContexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> context) : base(context)
    {
    }

    public DbSet<UserModel>? Users { get; set; }
    public DbSet<RoleModel>? Roles { get; set; }
}