using Application.Controllers;
using Application.Services.Contracts;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers;
public class RolesController : BaseAdminController
{
    private readonly IRoleService _service;
    public RolesController(IRoleService service) => _service = service;
    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());
    [HttpGet]
    public async Task<IActionResult> Editor(string? id) => View("Editor", await _service.GetByIdAsync(id));
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editor(RoleDto model)
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
