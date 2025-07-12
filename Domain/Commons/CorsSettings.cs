namespace Domain.Commons;

public class CorsSettings
{
    public bool AllowAnyMethod { get; set; }
    public bool AllowAnyHeader { get; set; }
    public bool AllowCredentials { get; set; }
    public bool AllowAnyOrigin { get; set; }
    public string[] Origins { get; set; } = Array.Empty<string>();
}