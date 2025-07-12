using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Extensions;

public static class DatabaseContextExtensions
{
    public static async Task<(int totalFound, int totalReplaced)> FindAndReplaceInAllStringColumnsAsync<T>(
        this DbContext context,
        string findText,
        string replaceText) where T : class
    {
        if (string.IsNullOrWhiteSpace(findText)) return (0, 0);
        var dbSet = context.Set<T>();
        var entities = await dbSet.ToListAsync(); // Load all records
        int totalFound = 0;
        int totalReplaced = 0;
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

        foreach (var entity in entities)
        {
            bool isModified = false;
            foreach (var property in properties)
            {
                var currentValue = property.GetValue(entity) as string;
                if (!string.IsNullOrEmpty(currentValue) && currentValue.Contains(findText))
                {
                    var count = currentValue.Split(new[] { findText }, StringSplitOptions.None).Length - 1;
                    totalFound += count;
                    var newValue = currentValue.Replace(findText, replaceText);
                    property.SetValue(entity, newValue);
                    totalReplaced += count;
                    isModified = true;
                }
            }
            if (isModified)
                context.Entry(entity).State = EntityState.Modified;
        }

        await context.SaveChangesAsync();

        return (totalFound, totalReplaced);
    }
}
