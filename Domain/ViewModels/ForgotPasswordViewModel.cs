using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    [EmailAddress]
    public string UserEmail { get; set; } = null!;
}
