using Domain.Commons;
using Localization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Dtos;

public class RoleDto : BaseDto
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Title { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public bool IsStatic { get; set; }
    [NotMapped]
    public List<RolePermissionDto> Permissions { get; set; } = null!;
}
