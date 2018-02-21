using System.Globalization;

namespace TCMB.Helper
{
    public static class ConvertHelper
    {
        public static decimal? ToDecimal(this string source)
        {
            decimal result;
            decimal.TryParse(source.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out result);

            if (result == 0) return null;

            return result;
        }

        public static int? ToInt(this string source)
        {
            int result;
            int.TryParse(source, out result);

            if (result == 0) return null;

            return result;
        }
    }
}