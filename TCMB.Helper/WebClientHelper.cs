using System.Net;

namespace TCMB.Helper
{
    public class WebClientHelper
    {
        public static string Download(string url)
        {
            string result = null;

            try
            {
                result = new WebClient().DownloadString(url);
            }
            catch
            {
                // ignored
            }

            return result;
        }
    }
}