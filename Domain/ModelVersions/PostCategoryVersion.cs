using Domain.Commons;
using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.ModelVersions;

public class PostCategoryVersion : BaseVersion
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Name { get; set; } = null!;
}
