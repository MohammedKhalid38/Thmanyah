using Application.Commons.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Commons;
using Newtonsoft.Json;
using System.Reflection;

namespace Application.Commons;

[RegisterClassAsScoped]
public class DynamicMapper : IDynamicMapper
{
    public object? Map(object? source, Type? sourceType, Type destinationType)
    {
        if (destinationType == null) throw new ArgumentNullException(nameof(destinationType));
        if (source == null || sourceType == null) return Activator.CreateInstance(destinationType);

        var destination = Activator.CreateInstance(destinationType);
        if (destination == null) return null;

        MapInternal(source, destination, sourceType, destinationType);
        return destination;
    }
    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        if (source == null || destination == null) return destination;
        MapInternal(source, destination, source.GetType(), destination.GetType());
        return destination;
    }
    public TDestination Map<TDestination>(object? source)
    {
        var mapped = Map(source, source?.GetType(), typeof(TDestination));
        return mapped != null ? (TDestination)mapped : Activator.CreateInstance<TDestination>();
    }
    public List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceList) where TDestination : class => sourceList == null ? new List<TDestination>() : sourceList.Select(sourceItem => Map<TDestination>(sourceItem)).ToList();
    #region Core shared mehtods
    private void MapInternal(object source, object destination, Type sourceType, Type destinationType)
    {
        var sourceProperties = GetProperties(sourceType);
        var destinationProperties = GetProperties(destinationType);
        foreach (var sourceProp in sourceProperties)
        {
            var destProp = destinationProperties.FirstOrDefault(p => p.Name == sourceProp.Name);
            if (destProp == null || !destProp.CanWrite) continue;
            try
            {
                var value = sourceProp.GetValue(source);
                if (IsMultilingualConversion(sourceProp.PropertyType, destProp.PropertyType))
                {
                    SetMultilingualValue(destProp, destination, value);
                    continue;
                }
                var sourceUnderlying = GetUnderlyingType(sourceProp.PropertyType);
                var destUnderlying = GetUnderlyingType(destProp.PropertyType);
                if (sourceUnderlying == destUnderlying)
                {
                    if (value != null || IsNullableType(destProp.PropertyType))
                        destProp.SetValue(destination, value);
                    else
                        destProp.SetValue(destination, Activator.CreateInstance(destProp.PropertyType));
                }
            }
            catch (Exception)
            {
                // Optionally log error for property mapping failures
            }
        }
    }
    #endregion

    #region Helper methods
    private static PropertyInfo[] GetProperties(Type type) => type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
    private static Type GetUnderlyingType(Type type) => Nullable.GetUnderlyingType(type) ?? type;
    private static bool IsNullableType(Type type) =>
        !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
    private static bool IsMultilingualConversion(Type sourceType, Type destType) =>
        (sourceType == typeof(List<MultilingualField>) && destType == typeof(string)) ||
        (sourceType == typeof(string) && destType == typeof(List<MultilingualField>));

    private static void SetMultilingualValue(PropertyInfo destProp, object destination, object? value)
    {
        if (destProp.PropertyType == typeof(string))
        {
            if (value != null)
                destProp.SetValue(destination, JsonConvert.SerializeObject(value));
        }
        else if (destProp.PropertyType == typeof(List<MultilingualField>))
        {
            var deserialized = JsonConvert.DeserializeObject<List<MultilingualField>>(value?.ToString() ?? string.Empty);
            destProp.SetValue(destination, deserialized);
        }
    }
    #endregion
}