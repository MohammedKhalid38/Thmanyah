using Application.Commons.Contracts;
using Application.IdentityServices.Contracts;
using Application.Services.Contracts;
using AutoDependencyRegistration.Attributes;
using Domain.Dtos;
using Domain.Enums;
using Domain.IdentityModels;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Application.Commons;

[RegisterClassAsScoped]
public class SessionProvider : ISessionProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly UserManager<User> _userManager;
    public string CurrentControllerName { get { return _httpContextAccessor.HttpContext?.Request.RouteValues["controller"]?.ToString() ?? string.Empty; } }
    public bool HasPublishPermission { get { return HasPermission(PermissionKey.Approve); } }
    public SessionProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        _userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
    }
    public string GetCurrentUrlWithQueryString() => $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}{_httpContextAccessor.HttpContext?.Request.Path}{_httpContextAccessor.HttpContext?.Request.QueryString}";
    public string GetCurrentController() => _httpContextAccessor.HttpContext?.Request.RouteValues["controller"]?.ToString() ?? string.Empty;
    public Guid GetCurrentLocaleId() => GetLocale()?.Id ?? Guid.Empty;
    public string GetCurrentLocale() => _httpContextAccessor.HttpContext?.Session?.GetString("Locale")?.ToLowerInvariant() ?? "ar";
    public string GetCurrentDirection() => _httpContextAccessor.HttpContext?.Session.GetString("Direction")?.ToLower() ?? "rtl";
    public LocaleDto? GetLocale()
    {
        var _localeService = _serviceProvider.GetRequiredService<ILocaleService>();
        return _localeService.Search(f => f.Code == GetCurrentLocale()).FirstOrDefault();
    }
    public bool IsArabic() => GetCurrentLocale() == "ar";
    public bool IsRightDirection() => GetCurrentDirection() == "rtl";
    public Guid GetCurrentUserGuid()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userId, out var userGuid) ? userGuid : Guid.Empty;
    }
    public Guid GetCurrentDepartmentId() => GetLoggedInUserInfo()?.DepartmentId ?? Guid.Empty;
    public User? GetLoggedInUserInfo()
    {
        if (IsLoggedInUser())
        {
            var userSession = _httpContextAccessor.HttpContext?.Session.GetObject<User>("User");
            if (userSession != null)
                return userSession;

            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            var user = _userManager.Users.FirstOrDefault(f => f.UserName == userName);
            if (user != null)
            {
                _httpContextAccessor?.HttpContext?.Session.SetObject("User", user);
                return user;
            }
        }
        return null;

    }
    public bool IsLoggedInUser() => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    public List<PermissionDto> GetLoggedInUserPermissions()
    {
        var _rolePermissionService = _serviceProvider.GetRequiredService<IRolePermissionService>();
        var _permissionService = _serviceProvider.GetRequiredService<IPermissionService>();
        List<PermissionDto> permissions = new();
        if (IsLoggedInUser())
        {
            var sessionPermissions = _httpContextAccessor.HttpContext?.Session?.GetObject<List<PermissionDto>>("Permissions");
            if (sessionPermissions != null && sessionPermissions.Count > 0)
                permissions = sessionPermissions;
            else
            {
                var user = GetLoggedInUserInfo();
                Guid roleId = user?.RoleId ?? Guid.Empty;
                foreach (var permission in _rolePermissionService.Search(s => s.RoleId == roleId))
                    permissions.Add(_permissionService.GetPublishById(permission.PermissionId));

                _httpContextAccessor.HttpContext?.Session.SetObject("Permissions", permissions);
            }
        }
        return permissions;

    }
    public bool HasPermission(PermissionKey permission)
    {

        var test = GetLoggedInUserPermissions();

        var x = test.Exists(a =>
    string.Equals(a.Name, permission.ToString(), StringComparison.OrdinalIgnoreCase) ||
    string.Equals(a.Name, nameof(PermissionKey.FullControl), StringComparison.OrdinalIgnoreCase));

        return x;


    }
    public string GetBaseUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        return request != null ? $"{request.Scheme}://{request.Host.Value}{request.PathBase.Value?.ToString()}" : string.Empty;
    }
    public string GetUrlQueryStringValue(string key)
    {
        try
        {
            return _httpContextAccessor.HttpContext?.Request.Query[key].ToString() ?? string.Empty;
        }
        catch { return string.Empty; }
    }
}
