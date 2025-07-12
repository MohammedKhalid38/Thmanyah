using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Providers.Contracts;

public interface IFolderManagerProvider
{
    Task<string?> UploadFile(IFormFile file);
    bool IsFileExist(string folderPath);
    bool DeleteFile(string filePath);
    string ConvertSizeToByteFamilySize(long bytes);
    string GetHtmlTemplate(string templateName);
    string[] GetAllowedExtensions(MediaFileType fileType);
    string[] GetDocumentExtensions();
    string[] GetFileExtensions();
    string[] GetImageExtensions();
    string[] GetVideoExtensions();
    long GetAllowedFileSize();
}
