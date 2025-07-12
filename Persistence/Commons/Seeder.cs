using Microsoft.EntityFrameworkCore;

namespace Persistence.Commons;

public abstract class Seeder
{
    public abstract void SeedData(ModelBuilder modelBuilder);
}
