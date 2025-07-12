using Domain.Enums;

namespace Infrastructure.Extensions;

public static class YesNoAnswersExtensions
{
    public static string GetValue(this YesNoAnswers yesNoAnswers, string locale) => locale == "ar" ? (yesNoAnswers == YesNoAnswers.Yes ? "نعم" : "لا") : yesNoAnswers.ToString();
}
