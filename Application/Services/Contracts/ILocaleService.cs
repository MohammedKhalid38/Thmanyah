using Application.Commons.Contracts;
using Domain.Commons;
using Domain.Dtos;
using Domain.Models;

namespace Application.Services.Contracts;

public interface ILocaleService : IBaseMainService<LocaleDto, Locale>
{
    IEnumerable<LocaleDto> GetAllAvailableLanguages();
    IEnumerable<LocaleDto> GetAllActiveLanguages();
    LocaleDto GetByCode(string code);
    Task<ResultResponse> AddLanguageAsync(Guid id);
    string ChangeLanguage(string locale);

}
