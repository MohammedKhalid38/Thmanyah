using Domain.Commons;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Extensions;

public static class StringExtensions
{
    public static Guid ToGuid(this string? text) => !string.IsNullOrEmpty(text) ? Guid.Parse(text) : Guid.Empty;
    public static bool IsContainsMultilingual(this string? text) => text?.Contains("Multilingual") ?? false;
    public static string? GetValue(this string field)
    {
        try
        {
            var locale = new HttpContextAccessor().HttpContext?.Session.GetString("Locale");
            var result = JsonConvert.DeserializeObject<List<MultilingualField>>(field ?? string.Empty);

            return (!string.IsNullOrEmpty(locale)) ? result?.GetDefaultValue(locale) : result?.GetDefaultValue("ar");
        }
        catch
        {
            return string.Empty;
        }
    }
    public static string? GetValue(this string field, string? locale = null)
    {
        try
        {
            locale ??= new HttpContextAccessor().HttpContext?.Session.GetString("Locale") ?? "ar";
            var result = JsonConvert.DeserializeObject<List<MultilingualField>>(field ?? string.Empty);
            return result?.GetDefaultValue(locale) ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public static List<MultilingualField>? ToMultilingualFieldList(this string field) => JsonConvert.DeserializeObject<List<MultilingualField>>(field ?? string.Empty);

    public static string TagifyToString(this string? text) => text?.Replace("{\"value\":\"", string.Empty).Replace("\"}", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty) ?? string.Empty;
    public static string[] TagifyToArrayString(this string? text)
    {
        string[] strings = Array.Empty<string>();
        if (!string.IsNullOrEmpty(text))
            strings = text.TagifyToString().Split(",");

        return strings;
    }
    public static string[] ToArrayString(this string input) => input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

    public static string ToHtmlList(this string input)
    {
        if (!string.IsNullOrEmpty(input.Trim()))
        {
            var lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            StringBuilder html = new StringBuilder();

            html.Append("<ul>");

            foreach (var line in lines)
            {
                if (line.Trim().StartsWith("•"))
                {
                    string content = line.Trim().Substring(1).Trim(); // Remove the bullet point and trim spaces
                    html.Append($"<li>{content}</li>");
                }
                else
                {
                    html.Append($"<li>{line}</li>");
                }
            }
            html.Append("</ul>");
            return html.ToString();
        }

        return string.Empty;

    }
}