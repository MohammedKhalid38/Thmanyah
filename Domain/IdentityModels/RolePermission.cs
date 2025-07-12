using Domain.Commons;

namespace Domain.IdentityModels;
public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}
