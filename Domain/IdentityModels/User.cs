using Localization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.IdentityModels;
public class User : IdentityUser<Guid>
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Name { get; set; } = null!;
    public Guid? Avatar { get; set; }
    public Guid? RoleId { get; set; }
    public Guid? DepartmentId { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public bool IsDeleted { get; set; }
    public Role? Role { get; set; }

}