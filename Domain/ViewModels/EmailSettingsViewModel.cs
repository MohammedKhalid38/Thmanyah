namespace Domain.ViewModels;

public class EmailSettingsViewModel
{
    public string EmailFrom { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string AuthUser { get; set; } = null!;
    public string AuthPassword { get; set; } = null!;
}
