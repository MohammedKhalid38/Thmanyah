namespace Domain.Commons.Contracts;

public interface IBaseVersion
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public Guid? PublishedBy { get; set; }
    public Guid MainVersionId { get; set; }
}
