using Application.Commons.Contracts;
using Application.IdentityServices.Contracts;
using Application.Services.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Commons;
using Domain.Dtos;
using Domain.IdentityModels;
using Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

[RegisterClassAsScoped]
public class RoleService : IRoleService
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IRolePermissionService _rolePermissionService;
    private readonly IUserService _userService;
    private readonly IDynamicMapper _dynamicMapper;
    public RoleService(RoleManager<Role> roleManager, IDynamicMapper dynamicMapper, IRolePermissionService rolePermissionService, IUserService userService)
    {
        _roleManager = roleManager;
        _dynamicMapper = dynamicMapper;
        _rolePermissionService = rolePermissionService;
        _userService = userService;
    }
    public async Task<IEnumerable<RoleDto>> GetAllAsync() => _dynamicMapper.MapList<Role, RoleDto>(await _roleManager.Roles.Where(w => w.IsDeleted == false).ToListAsync());
    public async Task<RoleDto> GetByIdAsync(string? roleId) => !string.IsNullOrEmpty(roleId) ? _dynamicMapper.Map<RoleDto>(await _roleManager.FindByIdAsync(roleId)) : new RoleDto();
    public async Task<ResultResponse> SaveAsync(RoleDto model, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return ResultResponse.Set(false, Resources.PleaseCorrectTheValidationErrors, modelState);

        var entity = _dynamicMapper.Map<Role>(model);
        if (entity.Id == Guid.Empty)
            await _roleManager.CreateAsync(entity);
        else
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            await SaveRolePermissions(entity.Id, model.Permissions);
            await _roleManager.UpdateAsync(_dynamicMapper.Map(model, role));
        }
        return ResultResponse.Set(true, Resources.DataSavedSuccessfully);
    }
    private async Task SaveRolePermissions(Guid roleId, List<RolePermissionDto> permissions)
    {
        foreach (var item in permissions)
        {
            if (item.IsSelected)
            {
                var permission = (await _rolePermissionService.SearchAsync(f => f.RoleId == roleId && f.PermissionId == item.PermissionId)).FirstOrDefault();
                if (permission == null)
                    await _rolePermissionService.SaveAsync(new RolePermissionDto { RoleId = roleId, PermissionId = item.PermissionId, IsActive = true, IsPublished = true });
                else
                    await _rolePermissionService.RestoreAsync(permission.Id);
            }
            else
            {
                var permission = (await _rolePermissionService.SearchAsync(f => f.RoleId == item.RoleId && f.PermissionId == item.PermissionId)).FirstOrDefault();
                if (permission != null)
                    await _rolePermissionService.DeleteAsync(permission.Id);
            }
        }
    }
    public async Task<int> GetUsersInRoleAsync(Guid? roleId) => (await _userService.GetAllAsync()).Count(w => w.RoleId == (roleId ?? Guid.Empty));
    public async Task<ResultResponse> DeleteAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return ResultResponse.Set(false, Resources.DataNotFound);

        role.IsDeleted = true;
        await _roleManager.UpdateAsync(role);
        return ResultResponse.Set(true, Resources.DataDeletedSuccessfully);
    }
    //public IEnumerable<Role> Search(Expression<Func<Role, bool>> predicate) => _roleManager.Roles.Where(predicate).ToList();
}
