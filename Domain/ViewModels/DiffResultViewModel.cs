using Domain.Enums;

namespace Domain.ViewModels;

public class DiffResultViewModel
{
    public string Text { get; set; } = null!;
    public DiffType Type { get; set; }
}
