using AutoDependencyRegistration.Attributes;
using Domain.ViewModels;
using Infrastructure.Providers.Contracts;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Providers;

[RegisterClassAsScoped]
public class EmailProvider : IEmailProvider
{
    private readonly EmailSettingsViewModel _emailSettings;
    public EmailProvider(IConfiguration configuration)
    {
        _emailSettings = new EmailSettingsViewModel()
        {
            EmailFrom = configuration["EmailAndSMS:EmailFrom"] ?? string.Empty,
            Password = configuration["EmailAndSMS:Password"] ?? string.Empty,
            AuthUser = configuration["EmailAndSMS:AuthUser"] ?? string.Empty,
            AuthPassword = configuration["EmailAndSMS:AuthPassword"] ?? string.Empty
        };

    }
    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        return await SendEmailAsync(new EmailMessageViewModel { To = to, Subject = subject, Body = body });
    }
    public async Task<bool> SendEmailAsync(EmailMessageViewModel emailMessage)
    {
        //try
        //{
        //    using EmailServiceClient proxy = new();
        //    using (new OperationContextScope(proxy.InnerChannel))
        //    {
        //        MessageHeader head = MessageHeader.CreateHeader("Authorization", "SendEmail", "Basic " + _emailSettings.AuthUser + ":" + _emailSettings.AuthPassword + "");
        //        OperationContext.Current.OutgoingMessageHeaders.Add(head);
        //        await proxy.SendEmailMSGAsync("SBC", _emailSettings.Password, _emailSettings.EmailFrom, emailMessage.To, emailMessage.Subject, emailMessage.Body, null, null, null);
        //        proxy.Close();
        //        return true;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    _logger.Error(ex.Message);
        //}
        return await Task.FromResult(true);
    }
}