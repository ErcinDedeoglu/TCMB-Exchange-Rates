using System;

namespace TCMB.Helper
{
    public class DateHelper
    {
        public static long ShortDate(DateTime date)
        {
            return Convert.ToInt64(date.ToString("yyyyMMddHHmm"));
        }
    }
}