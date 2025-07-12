using Application.Commons;
using Application.Services.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Dtos;
using Domain.Models;
using Persistence.Contexts;

namespace Application.Services;
[RegisterClassAsScoped]
public class PostCategoryService : BaseMainService<PostCategoryDto, PostCategory>, IPostCategoryService
{
    public PostCategoryService(DataContext context, IServiceProvider serviceProvider) : base(context, serviceProvider) { }
}
