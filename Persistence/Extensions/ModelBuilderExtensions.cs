using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Persistence.Commons;
using System.Reflection;

namespace Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void RegisterAllEntities<BaseModel>(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes()).Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(BaseModel).IsAssignableFrom(c));
        foreach (Type item in types)
            modelBuilder.Entity(item).ToTable(Pluralizer.Pluralize(item.Name));
    }
    public static void SeedData(this ModelBuilder modelBuilder, params Assembly[] seeders)
    {
        IEnumerable<Type> types = seeders.SelectMany(a => a.GetExportedTypes()).Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(Seeder).IsAssignableFrom(c));
        foreach (Type item in types)
            ((Seeder?)Activator.CreateInstance(item))?.SeedData(modelBuilder);
    }
}