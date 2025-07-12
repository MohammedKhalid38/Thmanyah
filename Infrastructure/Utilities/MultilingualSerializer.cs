using Domain.Commons;
using Newtonsoft.Json;

namespace Infrastructure.Utilities;

public static class MultilingualSerializer
{
    public static string Serialize(List<MultilingualField>? model) => model != null ? JsonConvert.SerializeObject(model) : string.Empty;
    public static List<MultilingualField>? Deserialize(string model) => JsonConvert.DeserializeObject<List<MultilingualField>>(model);
}
