namespace Domain.ViewModels;

public class EntityHistoryViewModel
{
    public Guid Id { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? IsPublished { get; set; }
    public bool IsNeedDeleteApprove { get; set; }
    public bool IsMain { get; set; }
}
