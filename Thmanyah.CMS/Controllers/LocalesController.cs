using Application.Controllers;
using Application.Services.Contracts;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers;
public class LocalesController : BaseAdminController
{
    private readonly ILocaleService _service;
    public LocalesController(ILocaleService service) => _service = service;
    [HttpGet]
    public IActionResult Index() => View(_service.GetAllPublished());

    [HttpGet]
    public async Task<IActionResult> Editor(Guid? id) => View(await _service.GetByIdAsync(id));
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editor(LocaleDto model)
    {
        var result = _service.Save(model, ModelState);
        if (result.Success)
            return RedirectToAction(nameof(Index));

        SetAlert(result);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));
}
