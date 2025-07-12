using AutoDependencyRegistration.Attributes;
using Infrastructure.Providers.Contracts;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Providers;
[RegisterClassAsScoped]
public class FileProvider : IFileProvider
{
    public async Task<string?> UploadFile(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadPath = Path.Combine(wwwRootPath, "assets", "media");

            // Ensure directory exists
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Unique file name
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/assets/media/{uniqueFileName}";
        }
        return null;
    }
}
