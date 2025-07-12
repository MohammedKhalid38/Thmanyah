namespace Domain.ViewModels;

public class OtpViewModel
{
    public bool IsValidUser { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public Guid UserId { get; set; }
    public string Code { get; set; } = null!;
}

