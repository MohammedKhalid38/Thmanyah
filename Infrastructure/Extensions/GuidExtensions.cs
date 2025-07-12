namespace Infrastructure.Extensions;
public static class GuidExtensions
{
    public static bool IsNullOrEmpty(this Guid? guid) => guid == null || guid == Guid.Empty;
}
