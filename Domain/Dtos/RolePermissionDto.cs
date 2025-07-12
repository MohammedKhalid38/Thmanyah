using Domain.Commons;
using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class RolePermissionDto : BaseDto
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public Guid RoleId { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public Guid PermissionId { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public bool IsSelected { get; set; }
}
