using AutoDependencyRegistration.Attributes;
using Domain.Enums;
using Infrastructure.Providers.Contracts;
using Infrastructure.Utilities;
using Localization;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Providers;

[RegisterClassAsScoped]
public class FolderManagerProvider : IFolderManagerProvider
{
    private readonly ISettingProvider _settingProvider;
    public string WebRootPath { get; set; }
    public FolderManagerProvider(ISettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
        WebRootPath = $"{_settingProvider.GetSettingByKey(ApplicationSettings.SiteMediaMainFolder)}";// Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\");
    }
    public async Task<string?> UploadFile(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var uploadPath = Path.Combine("assets", "media");

            // Ensure directory exists
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Unique file name
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(uploadPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/assets/media/{uniqueFileName}";
        }
        return null;
    }
    public bool IsFileExist(string filePath) => File.Exists($"{WebRootPath}{filePath}");
    public bool DeleteFile(string filePath)
    {
        var fullPath = Path.Combine(WebRootPath, filePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }
        return false;
    }
    public string GetHtmlTemplate(string templateName)
    {
        var templatePath = Path.Combine(WebRootPath, "email-templates", $"{templateName}.html");
        return File.ReadAllText(templatePath);
    }
    public string ConvertSizeToByteFamilySize(long bytes)
    {
        string result;
        if (bytes < 1024)
            result = $"{bytes} {Resources.Bytes}";
        else
        {
            double kilobytes = bytes / 1024.0;
            double megabytes = kilobytes / 1024.0;
            double gigabytes = megabytes / 1024.0;
            if (kilobytes < 1024)
                result = $"{kilobytes:N2} {Resources.KiloBytes}";
            else if (megabytes < 1024)
                result = $"{megabytes:N2} {Resources.MegaBytes}";
            else
                result = $"{gigabytes:N2} {Resources.GigaBytes}";
        }

        return result;
    }
    public string[] GetFileExtensions()
    {
        List<string> extensions = new();
        foreach (string item in _settingProvider.GetSettingByKey(ApplicationSettings.MediaFileExtensions).Split(','))
            extensions.Add(item);

        return extensions.ToArray();
    }
    public string[] GetDocumentExtensions()
    {
        List<string> extensions = new();
        foreach (string item in _settingProvider.GetSettingByKey(ApplicationSettings.MediaFileDocumentExtensions).Split(','))
            extensions.Add(item);

        return extensions.ToArray();
    }
    public string[] GetImageExtensions()
    {
        List<string> extensions = new();
        foreach (string item in _settingProvider.GetSettingByKey(ApplicationSettings.MediaFileImageExtensions).Split(','))
            extensions.Add(item);

        return extensions.ToArray();
    }
    public string[] GetVideoExtensions()
    {
        List<string> extensions = new();
        foreach (string item in _settingProvider.GetSettingByKey(ApplicationSettings.MediaFileVideoExtensions).Split(','))
            extensions.Add(item);

        return extensions.ToArray();
    }
    public string[] GetAllowedExtensions(MediaFileType fileType)
    {
        List<string> extensions = new();

        if (fileType == MediaFileType.Document)
            foreach (string item in GetDocumentExtensions().ToList())
                extensions.Add(item);
        else if (fileType == MediaFileType.Image)
            foreach (string item in GetImageExtensions().ToList())
                extensions.Add(item);
        else if (fileType == MediaFileType.Video)
            foreach (string item in GetVideoExtensions().ToList())
                extensions.Add(item);
        else
            foreach (string item in GetFileExtensions().ToList())
                extensions.Add(item);

        return extensions.ToArray();
    }
    public long GetAllowedFileSize()
    {
        int maxMediaFileSizeInMB = !string.IsNullOrEmpty(_settingProvider.GetSettingByKey(ApplicationSettings.MaxMediaFileSizeInMB)) ? int.Parse(_settingProvider.GetSettingByKey(ApplicationSettings.MaxMediaFileSizeInMB)) : 0;
        return maxMediaFileSizeInMB * 1024 * 1024;
    }


}
