using Domain.Commons;

namespace Infrastructure.Extensions;

public static class DtoExtensions
{
    public static string GetName(this BaseDto? dto)
    {
        if (dto != null)
        {
            foreach (var property in dto.GetType().GetProperties())
            {
                if (property.Name.Contains("Title"))
                {
                    if (property == null) return string.Empty;
                    if (property.PropertyType == typeof(List<MultilingualField>))
                        return ((List<MultilingualField>?)property.GetValue(dto))?.GetValue() ?? string.Empty;
                    else if (property.PropertyType == typeof(string))
                        return property.GetValue(dto)?.ToString() ?? string.Empty;

                }
                else if (property.Name.Contains("Name"))
                {
                    if (property == null) return string.Empty;
                    if (property.PropertyType == typeof(List<MultilingualField>))
                        return ((List<MultilingualField>?)property.GetValue(dto))?.GetValue() ?? string.Empty;
                    else if (property.PropertyType == typeof(string))
                        return property.GetValue(dto)?.ToString() ?? string.Empty;
                }
            }
        }
        return string.Empty;
    }
}
