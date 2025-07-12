using Application.Controllers;
using Application.Services.Contracts;
using Domain.Dtos;
using Infrastructure.Providers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers;
public class PostsController : BaseAdminController
{
    private readonly IPostService _service;
    private readonly IFileProvider _fileProvider;
    public PostsController(IPostService service, IFileProvider fileProvider)
    {
        _service = service;
        _fileProvider = fileProvider;
    }
    [HttpGet]
    public IActionResult Index() => View(_service.GetAllPublished());

    [HttpGet]
    public async Task<IActionResult> Editor(Guid? id) => View(await _service.GetByIdAsync(id));
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editor(PostDto model, IFormFile? imageFile)
    {
        if (imageFile != null)
        {
            string? imagePath = await _fileProvider.UploadFile(imageFile);
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.Image = imagePath;
                ModelState.Remove("Image");
            }
        }
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
