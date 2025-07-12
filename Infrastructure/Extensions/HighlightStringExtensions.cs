using System.Text.RegularExpressions;

namespace Infrastructure.Extensions;

public static class HighlightStringExtensions
{
    public static string TruncateAndHighlight(this string text, string searchTerm, int maxLength = 150)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchTerm))
            return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;

        int queryIndex = text.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase);

        if (queryIndex <= maxLength)
        {
            int endIndex = Math.Min(text.Length, maxLength);
            var plainText = text.Substring(0, endIndex);
            return Highlight(plainText, searchTerm) + (text.Length > maxLength ? "..." : "");
        }
        int startIndex = Math.Max(0, queryIndex - 50);
        int adjustedLength = Math.Min(text.Length - startIndex, maxLength);
        var snippet = text.Substring(startIndex, adjustedLength);
        return (startIndex > 0 ? "..." : "") + Highlight(snippet, searchTerm) + (text.Length > startIndex + adjustedLength ? "..." : "");
    }
    public static string Highlight(this string text, string searchTerm)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(searchTerm))
            return text;

        var regex = new Regex(Regex.Escape(searchTerm), RegexOptions.IgnoreCase);
        return regex.Replace(text, match => $"<span class=\"highlight\">{match.Value}</span>");
    }

}
