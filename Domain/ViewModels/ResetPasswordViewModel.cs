using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels;

public class ResetPasswordViewModel
{
    public string UserId { get; set; } = null!;

    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;

    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}
