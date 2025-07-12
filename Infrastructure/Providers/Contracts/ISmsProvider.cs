namespace Infrastructure.Providers.Contracts;

public interface ISmsProvider
{
    Task<bool> SendSmsAsync(string phoneNumber, string message);
}