using Domain.Commons;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Services.Contracts;
public interface IUserService
{
    Task<ResultResponse> SaveAsync(UserDto model, ModelStateDictionary modelState);
    Task<UserDto> GetUserByIdAsync(Guid userId);
    UserDto GetUserById(string userId);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<ResultResponse> DeleteAsync(string userId);
}
