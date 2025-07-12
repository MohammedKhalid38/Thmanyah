using Application.Commons;
using Application.Services.Contracts;
using Domain.Commons;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System.Globalization;

namespace Application.Services;

public class LocaleService : BaseMainService<LocaleDto, Locale>, ILocaleService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public LocaleService(DataContext context, IServiceProvider serviceProvider) : base(context, serviceProvider) => _httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
    public IEnumerable<LocaleDto> GetAllActiveLanguages() => Search(x => x.IsActive == true).OrderBy(o => o.Sort).ToList();
    public IEnumerable<LocaleDto> GetAllAvailableLanguages() => Search(x => x.IsActive == false).OrderBy(o => o.Sort).ToList();
    public LocaleDto GetByCode(string code) => _dynamicMapper.Map<LocaleDto>(_context.Set<Locale>().AsNoTracking().FirstOrDefault(f => f.Code == code));
    public async Task<ResultResponse> AddLanguageAsync(Guid id) => await SaveAsync(await GetPublishByIdAsync(id));
    public string ChangeLanguage(string locale)
    {
        var model = Search(f => f.Code == locale && f.IsActive && !f.IsDeleted).Select(s => new { s.Direction }).FirstOrDefault();
        if (model != null)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("Locale", locale);
            var cultureInfo = new CultureInfo(locale);
            if (cultureInfo.DateTimeFormat.Calendar is not GregorianCalendar)
            {
                cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();
            }
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            if (model is not null)
            {
                _httpContextAccessor.HttpContext?.Session.SetString("Direction", model.Direction.ToLower());
            }
        }
        // return _httpContextAccessor.HttpContext?.Request.GetDisplayUrl() ?? "/";
        return _httpContextAccessor.HttpContext?.Request.Headers["Referer"].ToString() ?? "/";
    }
}
