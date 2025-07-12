using Domain.Commons;
using Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[AllowAnonymous]
//[SetCultureFilter]
public class BaseController : Controller
{
    protected void SetAlert(ResultResponse response)
    {
        TempData["AlertMessage"] = response.Message;
        TempData["AlertStatus"] = response.Status;
        TempData["AlertType"] = response.Success;
    }
    protected void SetAlert(bool success, string message)
    {
        TempData["AlertMessage"] = message;
        TempData["AlertStatus"] = success ? Resources.Success : Resources.Error;
        TempData["AlertType"] = success;
    }
}