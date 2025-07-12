using Application.Commons.Contracts;
using Domain.Dtos;
using Domain.IdentityModels;

namespace Application.IdentityServices.Contracts;

public interface IRolePermissionService : IBaseMainService<RolePermissionDto, RolePermission> { }
