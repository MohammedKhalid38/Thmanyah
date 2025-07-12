using Application.Commons.Contracts;
using Domain.Dtos;
using Domain.Models;

namespace Application.Services.Contracts;

public interface IPostCategoryService : IBaseMainService<PostCategoryDto, PostCategory> { }
