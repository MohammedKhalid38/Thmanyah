using Application.Controllers;
using Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers;
public class HomeController : BaseAdminController
{
    private readonly IPostService _postService;
    public HomeController(IPostService postService)
    {
        _postService = postService;
    }
    public IActionResult Index() => View();
    [HttpPost]
    public IActionResult List(string query, Guid? postCategoryId) => PartialView("_List", _postService.GetAllPosts(query, postCategoryId));
}
