using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Infrastructure.Extensions;

public static class SessionExtensions
{
    public static void SetObject(this Microsoft.AspNetCore.Http.ISession session, string key, object value) => session.SetString(key, JsonConvert.SerializeObject(value));
    public static T? GetObject<T>(this Microsoft.AspNetCore.Http.ISession session, string key) => JsonConvert.DeserializeObject<T>(session.GetString(key) ?? string.Empty);
}