using Application.Controllers;
using Application.Services.Contracts;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers;
public class UsersController : BaseAdminController
{
    private readonly IUserService _service;
    public UsersController(IUserService service) => _service = service;
    [HttpGet]
    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    [HttpGet]
    public IActionResult Editor(string? id) => View(_service.GetUserById(id ?? string.Empty));
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editor(UserDto model)
    {
        var result = await _service.SaveAsync(model, ModelState);
        if (result.Success)
            return RedirectToAction(nameof(Index));

        SetAlert(result);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(string id) => Ok(await _service.DeleteAsync(id));
}
