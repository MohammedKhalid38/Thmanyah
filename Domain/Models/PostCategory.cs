using Domain.Commons;
using Localization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class PostCategory : BaseEntity
{
    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ThisFieldIsRequired")]
    public string Name { get; set; } = null!;
}
