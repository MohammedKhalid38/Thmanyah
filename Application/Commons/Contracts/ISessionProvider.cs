using Domain.Dtos;
using Domain.Enums;
using Domain.IdentityModels;

namespace Application.Commons.Contracts;

public interface ISessionProvider
{
    string CurrentControllerName { get; }
    bool HasPublishPermission { get; }
    string GetCurrentUrlWithQueryString();
    string GetUrlQueryStringValue(string key);
    string GetBaseUrl();
    Guid GetCurrentLocaleId();
    string GetCurrentLocale();
    string GetCurrentController();
    string GetCurrentDirection();
    LocaleDto? GetLocale();
    bool IsArabic();
    bool IsRightDirection();
    Guid GetCurrentUserGuid();
    Guid GetCurrentDepartmentId();
    User? GetLoggedInUserInfo();
    bool IsLoggedInUser();
    List<PermissionDto> GetLoggedInUserPermissions();
    bool HasPermission(PermissionKey permission);
}
