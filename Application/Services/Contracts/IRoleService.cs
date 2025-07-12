using Domain.Commons;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Services.Contracts;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto> GetByIdAsync(string? userId);
    Task<ResultResponse> SaveAsync(RoleDto model, ModelStateDictionary modelState);
    Task<ResultResponse> DeleteAsync(string userId);
    Task<int> GetUsersInRoleAsync(Guid? roleId);
}
