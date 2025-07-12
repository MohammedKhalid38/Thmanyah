using Application.Commons.Contracts;
using Application.Services.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Commons;
using Domain.Dtos;
using Domain.IdentityModels;
using Infrastructure.Extensions;
using Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;


[RegisterClassAsScoped]
public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IDynamicMapper _dynamicMapper;
    public UserService(UserManager<User> userManager, IDynamicMapper dynamicMapper)
    {
        _userManager = userManager;
        _dynamicMapper = dynamicMapper;
    }
    public async Task<UserDto> GetUserByIdAsync(Guid userId) => (userId != Guid.Empty) ? _dynamicMapper.Map<UserDto>(await _userManager.FindByIdAsync(userId.ToString())) : new UserDto();
    public UserDto GetUserById(string userId) => _dynamicMapper.Map<UserDto>(_userManager.Users.FirstOrDefault(f => f.Id == userId.ToGuid())) ?? new UserDto();
    public async Task<IEnumerable<UserDto>> GetAllAsync() => _dynamicMapper.MapList<User, UserDto>(await _userManager.Users.Where(w => w.IsDeleted == false).ToListAsync());
    public async Task<ResultResponse> SaveAsync(UserDto model, ModelStateDictionary modelState)
    {

        if (!modelState.IsValid)
            return ResultResponse.Set(false, Resources.PleaseCorrectTheValidationErrors, modelState);

        var entity = _dynamicMapper.Map<User>(model);
        entity.UserName = model.Email;
        if (entity.Id == Guid.Empty)
        {
            var result = (!string.IsNullOrEmpty(model.Password) && model.Password == model.ConfirmPassword) ? await _userManager.CreateAsync(entity, model.ConfirmPassword) : await _userManager.CreateAsync(entity);
            if (result.Succeeded)
                return ResultResponse.Set(true, Resources.DataSavedSuccessfully);
        }
        else
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            var result = await _userManager.UpdateAsync(_dynamicMapper.Map(model, user));
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Password))
                {
                    if (model.Password != model.ConfirmPassword)
                        return ResultResponse.Set(false, Resources.PasswordAndConfirmPasswordDoNotMatch);

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var changePasswordResult = await _userManager.ResetPasswordAsync(user, token, model.ConfirmPassword);
                    if (!changePasswordResult.Succeeded)
                        return ResultResponse.Set(false, Resources.PasswordAndConfirmPasswordDoNotMatch);
                }
            }
            return ResultResponse.Set(true, Resources.DataSavedSuccessfully);
        }
        return ResultResponse.Set(false, Resources.PasswordAndConfirmPasswordDoNotMatch, modelState);
    }
    public async Task<ResultResponse> DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return ResultResponse.Set(false, Resources.DataNotFound);

        user.IsDeleted = true;
        await _userManager.UpdateAsync(user);
        return ResultResponse.Set(true, Resources.DataDeletedSuccessfully);
    }
}
