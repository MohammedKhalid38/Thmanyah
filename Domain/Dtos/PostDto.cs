using Domain.Commons;
using Domain.Enums;
using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class PostDto : BaseDto
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Title { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Description { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Link { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Image { get; set; } = null!;
    public string? Meta { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Duration { get; set; } = null!;
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public PostType Type { get; set; }
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public Guid PostCategoryId { get; set; }
}
