using System.Reflection;

namespace Infrastructure.Extensions;

public static class ObjectExtensions
{
    public static void SetProperty(this object target, string propertyName, object? value)
    {
        var prop = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.CanWrite)
            prop.SetValue(target, value);
    }
    public static object? GetPropertyValue(this object source, string propertyName)
    {
        var prop = source.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
        return prop?.GetValue(source);
    }
}
