namespace Infrastructure.Utilities;

public static class Pluralizer
{
    private static readonly Dictionary<string, string> IrregularPlurals = new Dictionary<string, string>
    {
        {"man", "men"},
        {"woman", "women"},
        {"fish", "fish"},
        {"sheep", "sheep"},
        {"rice", "rice"},
        {"aircraft", "aircraft"},
        {"pyjamas", "pyjamas"},
        {"series", "series"},
        {"trout", "trout"},
        {"tuna", "tuna"},
        {"tooth", "teeth"},
        {"foot", "feet"},
        {"oasis", "oases"},
        {"goose", "geese"},
        {"child", "children"},
        {"person", "people"},
        {"mouse", "mice"},
        {"cactus", "cacti"},
        {"focus", "foci"},
        // Add more irregular plurals as needed
    };

    public static string Pluralize(string singular)
    {
        if (IrregularPlurals.ContainsKey(singular.ToLower())) return IrregularPlurals[singular.ToLower()];

        if (singular.EndsWith("y") && !singular.EndsWith("ay") && !singular.EndsWith("ey") && !singular.EndsWith("iy") && !singular.EndsWith("oy") && !singular.EndsWith("uy"))
            return singular.Remove(singular.Length - 1) + "ies";

        if (singular.EndsWith("s") || singular.EndsWith("x") || singular.EndsWith("z") || singular.EndsWith("sh") || singular.EndsWith("ch"))
            return singular + "es";

        return singular + "s";
    }
}
