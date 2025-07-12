using Domain.Commons.Contracts;

namespace Domain.Commons;

public abstract class BaseDto : IBaseDto
{
    public Guid Id { get; set; }
    public int Sort { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Guid? PublishedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool IsNeedDeleteApprove { get; set; }
    public bool IsActive { get; set; }
}