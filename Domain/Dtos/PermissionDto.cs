using Domain.Commons;
using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class PermissionDto : BaseDto
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Title { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public bool IsStatic { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public bool IsSelected { get; set; }

}
