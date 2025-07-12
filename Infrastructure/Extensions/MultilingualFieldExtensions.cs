using Domain.Commons;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions;

public static class MultilingualFieldExtensions
{
    //public static string? GetDefaultValue(this List<MultilingualField> list) => list.FirstOrDefault(f => f.Locale == "en")?.Value ?? list.FirstOrDefault()?.Value;
    public static string GetDefaultValue(this List<MultilingualField> list, string locale)
    {
        try
        {
            return list.FirstOrDefault(f => f.Locale == locale)?.Value ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
    public static string? GetValue(this List<MultilingualField> list)
    {
        string? locale;
        try
        {
            locale = new HttpContextAccessor().HttpContext?.Session.GetString("Locale");
        }
        catch
        {
            locale = "ar";
        }

        return (!string.IsNullOrEmpty(locale)) ? list.GetDefaultValue(locale) : list.GetDefaultValue("ar");
    }
    public static bool Includes(this List<MultilingualField> list, string? query) => string.IsNullOrEmpty(query) ? true : list.Any(a => string.IsNullOrEmpty(a.Value) || (a.Value?.Contains(query) ?? false));
    public static bool Peer(this List<MultilingualField> list, string? query) => string.IsNullOrEmpty(query) ? true : list.Any(a => string.IsNullOrEmpty(a.Value) || (a.Value == query));
    public static bool IsNullOrEmpty(this List<MultilingualField>? list) => list != null ? list.Count(a => string.IsNullOrEmpty(a.Value)) > 1 : true;
}