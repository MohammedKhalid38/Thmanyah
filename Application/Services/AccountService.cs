using Application.IdentityServices.Contracts;
using Application.Services.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Commons;
using Domain.Dtos;
using Domain.IdentityModels;
using Domain.ViewModels;
using Infrastructure.Extensions;
using Infrastructure.Providers.Contracts;
using Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

[RegisterClassAsScoped]
public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IPermissionService _permissionService;
    private readonly IRolePermissionService _rolePermissionService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailProvider _emailProvider;
    private readonly ISmsProvider _smsProvider;
    private readonly IFolderManagerProvider _folderManagerProvider;
    private readonly IConfiguration _configuration;
    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IHttpContextAccessor httpContextAccessor, IEmailProvider emailProvider, ISmsProvider smsProvider, IPermissionService permissionService, IRolePermissionService rolePermissionService, IFolderManagerProvider folderManagerProvider, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailProvider = emailProvider;
        _httpContextAccessor = httpContextAccessor;
        _smsProvider = smsProvider;
        _permissionService = permissionService;
        _rolePermissionService = rolePermissionService;
        _folderManagerProvider = folderManagerProvider;
        _configuration = configuration;
    }
    public async Task<ResultResponse> SignInAsync(LoginViewModel model, ModelStateDictionary modelState)
    {
        _httpContextAccessor.HttpContext?.Session.Clear();
        // Validate model
        if (!modelState.IsValid)
            return ResultResponse.Set(false, Resources.PleaseCorrectTheValidationErrors, modelState);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return ResultResponse.Set(false, Resources.UserNotFound);

        // Check password
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (!result.Succeeded)
            return ResultResponse.Set(false, Resources.UserNotFound);

        // Store user session
        _httpContextAccessor.HttpContext?.Session.SetObject("User", user);

        // Set role and permissions (if needed)
        SetCurrentRoleAndPermissions(user.RoleId);

        return ResultResponse.Set(true, Resources.AuthenticationSuccessfully);
    }


    //public async Task<ResultResponse> SignInAsync(Guid userId, ModelStateDictionary modelState)
    //{
    //    _httpContextAccessor.HttpContext?.Session.Clear();
    //    var user = await _userManager.FindByIdAsync(userId.ToString());
    //    if (user == null)
    //        return ResultResponse.Set(false, Resources.UserNotFound);

    //    await _signInManager.SignInAsync(user, isPersistent: true);
    //    _httpContextAccessor.HttpContext?.Session.SetObject("User", user);
    //    SetCurrentRoleAndPermissions(user.RoleId);
    //    return ResultResponse.Set(true, Resources.AuthenticationSuccessfully);
    //}
    public async Task<OtpViewModel> SendOtpCode(string email)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(f => f.Email == email && f.IsDeleted == false && f.LockoutEnabled == false);
        if (user == null)
            return new OtpViewModel { IsValidUser = false };


        string userNumber = user.PhoneNumber;
        string userEmail = user.Email;

        var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, userNumber);
        if (!string.IsNullOrEmpty(code))
        {
            string emailHtml = _folderManagerProvider.GetHtmlTemplate("otp-verification");
            emailHtml = emailHtml.Replace("{{user-name}}", user.Name).Replace("{{otp-code}}", code);
            string mobileMessageTemplate = string.Format(Resources.MobileVerficationMessage, code);
            await _smsProvider.SendSmsAsync(VerifyMobileNumber(userNumber), mobileMessageTemplate);
            await _emailProvider.SendEmailAsync(userEmail, Resources.EmailVerification, emailHtml);

            return new OtpViewModel { IsValidUser = true, UserId = user.Id, Email = userEmail, Phone = userNumber };
        }
        return new OtpViewModel { IsValidUser = false, UserId = Guid.Empty, Email = string.Empty, Phone = string.Empty };


    }
    public async Task<ResultResponse> VerifyOtpCode(OtpViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId.ToString());
        if (user == null)
            return ResultResponse.Set(false, Resources.UserNotFound);

        var valid = await _userManager.VerifyChangePhoneNumberTokenAsync(user, model.Code, user.PhoneNumber);
        if (!valid)
            return ResultResponse.Set(false, Resources.OtpInvalid);


        return ResultResponse.Set(true, Resources.AuthenticationSuccessfully);

    }
    public async Task SignOutAsync() => await _signInManager.SignOutAsync();
    public async Task<ResultResponse> ResetPasswordAsync(ResetPasswordViewModel model, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return ResultResponse.Set(false, Resources.PleaseCorrectTheValidationErrors);

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user is not null)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.ConfirmPassword);
            if (result.Succeeded)
                return ResultResponse.Set(true, Resources.ResetPasswordSuccessfully);
        }
        return ResultResponse.Set(false, Resources.FailedToResetPassword);
    }
    public async Task<ResultResponse> ForgotPasswordAsync(ForgotPasswordViewModel model, ModelStateDictionary modelState)
    {
        if (!modelState.IsValid)
            return ResultResponse.Set(false, Resources.PleaseCorrectTheValidationErrors);

        var user = await _userManager.FindByIdAsync(model.UserEmail);
        if (user is not null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"admin/account/reset/{user.Id}&token={token}";
            await _emailProvider.SendEmailAsync(user.Email, "Reset Password", $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
            return ResultResponse.Set(true, Resources.ForgetPasswordSuccessfully);
        }
        return ResultResponse.Set(false, Resources.UserNotFound);
    }
    private void SetCurrentRoleAndPermissions(Guid? id)
    {
        if (id != null)
        {
            _httpContextAccessor.HttpContext?.Session.SetObject("Role", _roleManager.Roles.FirstOrDefault(f => f.Id == id) ?? new Role());
            List<PermissionDto> permissions = new();
            foreach (var permission in _rolePermissionService.Search(s => s.RoleId == id))
            {
                permissions.Add(_permissionService.GetPublishById(permission.PermissionId));
            }
            _httpContextAccessor.HttpContext?.Session.SetObject("Permissions", permissions);
        }
    }
    private static string VerifyMobileNumber(string number) => number.Length == 9 ? $"0{number}" : number;
}
