namespace Infrastructure.Extensions;

public static class EnumerableExtensions
{
    public static void ReplaceAllColumns<T>(this IEnumerable<T> entities, string findText, string replaceText) where T : class
    {
        foreach (var entity in entities)
        {
            foreach (var prop in typeof(T).GetProperties().Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite))
            {
                var value = prop.GetValue(entity) as string;
                if (!string.IsNullOrEmpty(value) && value.Contains(findText))
                {
                    var replaced = value.Replace(findText, replaceText);
                    prop.SetValue(entity, replaced);
                }
            }
        }
    }
}
