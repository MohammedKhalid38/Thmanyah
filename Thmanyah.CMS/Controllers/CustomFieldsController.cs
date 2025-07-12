using Application.Controllers;
using Application.Services.Contracts;
using Domain.Commons;
using Domain.ViewModels;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers;

public class CustomFieldsController : BaseAdminController
{
    private readonly ILocaleService _localeService;
    public CustomFieldsController(ILocaleService localeService) => _localeService = localeService;
    [HttpPost]
    public IActionResult MultilingualField(string name, string type, string content, string cssClass, string cssStyle, bool isResizable = false)
    {
        List<MultilingualField> result = new();
        if (!string.IsNullOrEmpty(content))
        {
            var fields = MultilingualSerializer.Deserialize(content);
            foreach (var locale in _localeService.GetAllActiveLanguages())
                result.Add(new MultilingualField { Locale = locale.Code, Value = fields?.FirstOrDefault(f => f.Locale == locale.Code)?.Value ?? string.Empty });
        }
        MultilingualFieldViewModel model = new() { Name = name, Type = type, Fields = result, CssClass = cssClass, CssStyle = cssStyle, IsResizable = isResizable };
        return PartialView("_MultilingualField", model);
    }

    [HttpPost]
    public IActionResult FileUpload(string name, string type, string id, string cssClass, string cssStyle, string path)
    {
        FileUploadViewModel model = new() { Name = name, Type = type, Id = id, Path = path, CssClass = cssClass, CssStyle = cssStyle };
        return PartialView("_FileUpload", model);
    }
}
