using AutoDependencyRegistration.Attributes;
using Domain.Enums;
using Domain.ViewModels;
using HtmlAgilityPack;
using Infrastructure.Providers.Contracts;
using System.Web;

namespace Infrastructure.Providers;

[RegisterClassAsScoped]
public class HtmlProvider : IHtmlProvider
{
    public string Difference(string? oldHtml, string? newHtml)
    {
        var differences = DiffStrings(oldHtml ?? string.Empty, newHtml ?? string.Empty);
        // Rebuild the new HTML with highlights
        var resultHtml = "";
        foreach (var diff in differences)
        {
            if (diff.Type == DiffType.Addition)
            {
                // Highlight added text in green
                resultHtml += $"<span class=\"diff-string-add\">{HttpUtility.HtmlEncode(diff.Text)}</span>";
            }
            else if (diff.Type == DiffType.Deletion)
            {
                // Highlight deleted text in red
                resultHtml += $"<span class=\"diff-string-delete\">{HttpUtility.HtmlEncode(diff.Text)}</span>";
            }
            else
            {
                // Unchanged text
                resultHtml += HttpUtility.HtmlEncode(diff.Text);
            }
        }

        // Handle newlines explicitly
        resultHtml = resultHtml.Replace("\n", "<br>");


        // Ensure <ul> and <li> structure is preserved
        resultHtml = PreserveListStructure(resultHtml);



        var doc = new HtmlDocument();
        doc.LoadHtml(resultHtml);
        //return doc.DocumentNode.InnerText;
        var text = HtmlEntity.DeEntitize(doc.DocumentNode.OuterHtml);
        return text;
    }

    private string PreserveListStructure(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var ulNodes = doc.DocumentNode.SelectNodes("//ul");
        if (ulNodes != null)
        {
            foreach (var ulNode in ulNodes)
            {
                var liNodes = ulNode.SelectNodes("./li");
                if (liNodes != null)
                {
                    foreach (var liNode in liNodes)
                    {
                        // Process the inner HTML of each <li>
                        liNode.InnerHtml = HandleHighlightsInNode(liNode.InnerHtml);
                    }
                }
            }
        }

        return doc.DocumentNode.OuterHtml;
    }
    private string HandleHighlightsInNode(string innerHtml)
    {
        // Add handling for any nested elements or highlights within <li>
        // You can extend this as needed based on specific requirements
        return innerHtml;
    }
    private DiffResultViewModel[] DiffStrings(string oldText, string newText)
    {

        var oldWords = oldText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var newWords = newText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var diffs = new DiffMatchPatch.diff_match_patch();
        var diffResults = diffs.diff_main(oldText, newText);
        diffs.diff_cleanupSemantic(diffResults);

        return diffResults.Select(diff =>
        {
            return new DiffResultViewModel
            {
                Text = diff.text,
                Type = diff.operation == DiffMatchPatch.Operation.EQUAL ? DiffType.Unchanged :
                       diff.operation == DiffMatchPatch.Operation.INSERT ? DiffType.Addition : DiffType.Deletion
            };
        }).ToArray();
    }
}



