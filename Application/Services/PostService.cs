using Application.Commons;
using Application.Services.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Commons;
using Domain.Dtos;
using Domain.Models;
using Persistence.Contexts;

namespace Application.Services;

[RegisterClassAsScoped]
public class PostService : BaseMainService<PostDto, Post>, IPostService
{
    public PostService(DataContext context, IServiceProvider serviceProvider) : base(context, serviceProvider) { }


    public List<PostDto> GetAllPosts(string? query, Guid? postCategoryId)
    {
        return Search(s => (postCategoryId == null || s.PostCategoryId == postCategoryId) && (string.IsNullOrEmpty(query) || s.Title.Contains(query))).ToList();
    }
    public PaginatedResult<PostDto> GetAllPosts(string? query, Guid? postCategoryId, int pageNumber = 1, int pageSize = 10)
    {
        return Search(s => (postCategoryId == null || s.PostCategoryId == postCategoryId) && (string.IsNullOrEmpty(query) || s.Title.Contains(query)), pageNumber, pageSize);
    }
}
