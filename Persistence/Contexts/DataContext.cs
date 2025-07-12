using Domain.Commons;
using Domain.IdentityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Commons;
using Persistence.Extensions;

namespace Persistence.Contexts;

public class DataContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RegisterAllEntities<BaseEntity>(typeof(BaseEntity).Assembly);
        modelBuilder.RegisterAllEntities<BaseVersion>(typeof(BaseVersion).Assembly);
        modelBuilder.SeedData(typeof(Seeder).Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder builder) => base.OnConfiguring(builder);
}
