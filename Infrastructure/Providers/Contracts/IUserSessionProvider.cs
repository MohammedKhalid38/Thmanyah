namespace Infrastructure.Providers.Contracts;

public interface IUserSessionProvider
{
    Guid GetCurrentUserId();

}
