using Domain.Commons;
using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class PostCategoryDto : BaseDto
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public List<MultilingualField> Name { get; set; } = null!;
}
