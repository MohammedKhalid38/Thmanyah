using Application.Controllers;
using Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.API.Controllers;

public class SearchController : BaseApiController
{
    private readonly IPostService _postService;
    public SearchController(IPostService postService)
    {
        _postService = postService;
    }
    [HttpGet]
    public IActionResult Index(string? query, Guid? postCategoryId, int pageNumber = 1, int pageSize = 10) => Ok(_postService.GetAllPosts(query, postCategoryId, pageNumber, pageSize));

}
