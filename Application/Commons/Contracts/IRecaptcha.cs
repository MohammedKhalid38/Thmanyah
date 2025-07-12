namespace Application.Commons;

public interface IRecaptcha
{
    Task<bool> ValidateV2();
    Task<bool> ValidateV3(string token);
}
