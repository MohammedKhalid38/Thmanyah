using Domain.Commons;

namespace Domain.ViewModels;

public class MultilingualFieldViewModel
{
    public string Name { get; set; } = null!;
    public string? Content { get; set; }
    public string Type { get; set; } = null!;
    public bool IsResizable { get; set; }
    public List<MultilingualField> Fields { get; set; } = null!;
    public string? CssClass { get; set; }
    public string? CssStyle { get; set; }
}
