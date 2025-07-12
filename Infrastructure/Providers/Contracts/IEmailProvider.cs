using Domain.ViewModels;

namespace Infrastructure.Providers.Contracts;

public interface IEmailProvider
{
    Task<bool> SendEmailAsync(EmailMessageViewModel emailMessage);
    Task<bool> SendEmailAsync(string to, string subject, string body);
}