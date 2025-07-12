namespace Domain.ViewModels;

public class FileUploadViewModel
{
    public string Name { get; set; } = null!;
    public string? Id { get; set; }
    public string? Path { get; set; }
    public string Type { get; set; } = null!;
    public string? CssClass { get; set; }
    public string? CssStyle { get; set; }
}
