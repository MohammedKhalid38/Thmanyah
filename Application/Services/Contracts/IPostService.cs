using Application.Commons.Contracts;
using Domain.Commons;
using Domain.Dtos;
using Domain.Models;

namespace Application.Services.Contracts;

public interface IPostService : IBaseMainService<PostDto, Post>
{

    List<PostDto> GetAllPosts(string? query, Guid? postCategoryId);
    PaginatedResult<PostDto> GetAllPosts(string? query, Guid? postCategoryId, int pageNumber = 1, int pageSize = 10);
}
