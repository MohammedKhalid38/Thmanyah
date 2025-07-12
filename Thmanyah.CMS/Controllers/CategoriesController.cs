using Application.Controllers;
using Application.Services.Contracts;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers;

public class CategoriesController : BaseAdminController
{
    private readonly IPostCategoryService _service;
    public CategoriesController(IPostCategoryService service) => _service = service;
    [HttpGet]
    public IActionResult Index() => View(_service.GetAllPublished());

    [HttpGet]
    public async Task<IActionResult> Editor(Guid? id) => View(await _service.GetByIdAsync(id));
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editor(PostCategoryDto model)
    {
        var result = _service.Save(model, ModelState);
        if (result.Success)
            return RedirectToAction(nameof(Index));

        SetAlert(result);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));

    [HttpGet]
    public IActionResult Approvals() => View();
    [HttpGet]
    public IActionResult History(Guid id) => PartialView("_History", _service.HistoryVersions(id));
    [HttpGet]
    public IActionResult HistoryDetails(Guid id) => PartialView("_HistoryDetails", _service.HistoryDetails(id));
    [HttpPost]
    public IActionResult ConfirmPublish(Guid id) => Ok(_service.ConfirmPublish(id));
    [HttpPost]
    public IActionResult CancelPublish(Guid id) => Ok(_service.CancelPublish(id));
    [HttpPost]
    public IActionResult ConfirmDelete(Guid id) => Ok(_service.ConfirmDelete(id));
    [HttpPost]
    public IActionResult CancelDelete(Guid id) => Ok(_service.CancelDelete(id));
    [HttpPost]
    public IActionResult PublishAll() => Ok(_service.PublishAll());
    [HttpPost]
    public IActionResult DeleteAll() => Ok(_service.DeleteAll());
    [HttpPost]
    public IActionResult CancelDeleteAll() => Ok(_service.CancelDeleteAll());
    [HttpPost]
    public IActionResult CancelPublishAll() => Ok(_service.CancelPublishAll());
    [HttpPost]
    public async Task<IActionResult> Resort(Guid[] ids) => Ok(await _service.ResortAsync(ids));
    [HttpPost]
    public IActionResult PublishRequestCount() => Ok(_service.GetPublishAndDeleteRequestCount());
}
