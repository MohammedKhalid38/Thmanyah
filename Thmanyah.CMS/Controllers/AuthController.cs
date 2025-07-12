using Application.Commons;
using Application.Controllers;
using Application.Services.Contracts;
using Domain.ViewModels;
using Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers;

[AllowAnonymous]
public class AuthController : BaseAdminController
{
    private readonly IAccountService _accountService;
    private readonly IRecaptcha _recaptcha;
    public AuthController(IAccountService accountService, IRecaptcha recaptcha)
    {
        _accountService = accountService;
        _recaptcha = recaptcha;
    }
    public IActionResult Login() => (User?.Identity?.IsAuthenticated ?? false) ? RedirectToAction("Index", "Home") : View();
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {


            var result = await _accountService.SignInAsync(model, ModelState);
            if (result.Success)
                return RedirectToDashboardPage();
            else
                SetAlert(false, Resources.UserNotFound);

            //if (await _recaptcha.ValidateV3(recaptchaResponse))
            //{

            //}
            //else
            //    SetAlert(false, Resources.InvalidReCAPTCHA);
        }
        return View(model);
    }
    public async Task<IActionResult> Logout()
    {
        await _accountService.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
