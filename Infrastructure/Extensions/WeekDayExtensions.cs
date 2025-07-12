using Domain.Enums;

namespace Infrastructure.Extensions;

public static class WeekDayExtensions
{
    public static string GetValue(this WeekDay weekDay, string locale)
    {
        if (locale == "ar")
        {
            if (weekDay == WeekDay.Saturday)
                return "السبت";
            else if (weekDay == WeekDay.Sunday)
                return "الأحد";
            else if (weekDay == WeekDay.Monday)
                return "الإثنين";
            else if (weekDay == WeekDay.Tuesday)
                return "الثلاثاء";
            else if (weekDay == WeekDay.Wednesday)
                return "الأربعاء";
            else if (weekDay == WeekDay.Thursday)
                return "الخميس";
            else
                return "الجمعة";
        }
        else
        {
            return weekDay.ToString();
        }
    }
}
