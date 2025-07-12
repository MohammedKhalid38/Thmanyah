namespace Infrastructure.Providers.Contracts;

public interface IHtmlProvider
{
    string Difference(string? oldHtml, string? newHtml);
}
