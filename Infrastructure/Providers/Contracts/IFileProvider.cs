using Microsoft.AspNetCore.Http;

namespace Infrastructure.Providers.Contracts;

public interface IFileProvider
{
    Task<string?> UploadFile(IFormFile file);
}
