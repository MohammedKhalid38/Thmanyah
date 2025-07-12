using Application.Commons;
using Application.IdentityServices.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Dtos;
using Domain.IdentityModels;
using Persistence.Contexts;

namespace Application.IdentityServices;
[RegisterClassAsScoped]
public class RolePermissionService : BaseMainService<RolePermissionDto, RolePermission>, IRolePermissionService
{
    public RolePermissionService(DataContext context, IServiceProvider serviceProvider) : base(context, serviceProvider) { }
}
