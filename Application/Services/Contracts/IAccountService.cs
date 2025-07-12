using Domain.Commons;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Services.Contracts;

public interface IAccountService
{
    Task<ResultResponse> SignInAsync(LoginViewModel model, ModelStateDictionary modelState);
    Task SignOutAsync();
    Task<OtpViewModel> SendOtpCode(string email);
    Task<ResultResponse> VerifyOtpCode(OtpViewModel model);
    Task<ResultResponse> ResetPasswordAsync(ResetPasswordViewModel model, ModelStateDictionary modelState);
    Task<ResultResponse> ForgotPasswordAsync(ForgotPasswordViewModel model, ModelStateDictionary modelState);
}
