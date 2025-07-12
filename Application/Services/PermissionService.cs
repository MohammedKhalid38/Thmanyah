using Application.Commons;
using Application.IdentityServices.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Dtos;
using Domain.IdentityModels;
using Persistence.Contexts;

namespace Application.IdentityServices;

[RegisterClassAsScoped]
public class PermissionService : BaseMainService<PermissionDto, Permission>, IPermissionService
{
    public PermissionService(DataContext context, IServiceProvider serviceProvider) : base(context, serviceProvider) { }
}
