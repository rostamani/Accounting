using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Accounting.Utilities
{
    public static class DateConvertors
    {
        public static string ToPersianDate(this DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return string.Format("{0}/{1}/{2}",
                persianCalendar.GetYear(dateTime),
                persianCalendar.GetMonth(dateTime).ToString("00"),
                persianCalendar.GetDayOfMonth(dateTime).ToString("00"));
        }

        public static DateTime ToMiladi(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, new System.Globalization.PersianCalendar());
        }
    }
}
