using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Application.Commons;

public class Recaptcha : IRecaptcha
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public Recaptcha(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        _configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        _httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
    }
    public async Task<bool> ValidateV3(string token)
    {
        return await Task.FromResult(true);
        //var client = new HttpClient();
        //string secretKey = _configuration["GoogleRecaptcha:SecretKey"] ?? string.Empty;

        //var response = await client.PostAsync(
        //    "https://www.google.com/recaptcha/api/siteverify",
        //    new FormUrlEncodedContent(new[]
        //    {
        //        new KeyValuePair<string, string>("secret", secretKey),
        //        new KeyValuePair<string, string>("response", token)
        //    }));

        //var jsonResponse = await response.Content.ReadAsStringAsync();
        //var result = System.Text.Json.JsonSerializer.Deserialize<ReCaptchaVerificationResponse>(jsonResponse);

        //return result?.success == true;
    }

    public async Task<bool> ValidateV2()
    {
        string? encodedResponse = _httpContextAccessor.HttpContext?.Request.Form["g-recaptcha-response"];
        string remoteIpAddressRequest = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;

        if (!string.IsNullOrEmpty(encodedResponse) && !string.IsNullOrEmpty(remoteIpAddressRequest))
        {

            string remoteIpAddress = remoteIpAddressRequest != string.Empty && remoteIpAddressRequest != "::1" ? remoteIpAddressRequest : "localhost";
            var client = _httpClientFactory.CreateClient();
            try
            {
                string? secretKey = _configuration["GoogleRecaptcha:SecretKey"];
                var parameters = new Dictionary<string, string>
            {
                {"secret", secretKey ?? string.Empty},
                {"response", encodedResponse ?? string.Empty},
                {"remoteip",remoteIpAddress }
            };

                HttpResponseMessage response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                string apiResponse = await response.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);
                return (apiJson.success == true);
            }
            catch { }
        }

        return false;
    }
}
