using AutoDependencyRegistration.Attributes;
using Domain.ViewModels;
using Infrastructure.Providers.Contracts;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Providers;
[RegisterClassAsScoped]
public class SmsProvider : ISmsProvider
{
    private readonly EmailSettingsViewModel _emailSettings;
    //private readonly MessageHeader _messageHeader;
    public SmsProvider(IConfiguration configuration)
    {
        _emailSettings = new EmailSettingsViewModel()
        {
            AuthUser = configuration["EmailAndSMS:AuthUser"] ?? string.Empty,
            AuthPassword = configuration["EmailAndSMS:AuthPassword"] ?? string.Empty
        };
        // _messageHeader = MessageHeader.CreateHeader("Authorization", "SendEmail", "Basic " + _emailSettings.AuthUser + ":" + _emailSettings.AuthPassword + "");
    }
    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        return await Task.FromResult(false);
        //try
        //{
        //    using EmailServiceClient proxy = new();
        //    using (new OperationContextScope(proxy.InnerChannel))
        //    {
        //        OperationContext.Current.OutgoingMessageHeaders.Add(_messageHeader);
        //        return await proxy.SendSMSAsync("SBC", phoneNumber, message);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    _logger.Error(ex.Message);
        //}

        //return false;
    }
}