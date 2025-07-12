using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Password { get; set; } = null!;
}
