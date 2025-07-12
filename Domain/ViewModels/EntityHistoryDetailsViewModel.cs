namespace Domain.ViewModels;

public class EntityHistoryDetailsViewModel
{
    public object Current { get; set; } = null!;
    public object? Version { get; set; }
}
