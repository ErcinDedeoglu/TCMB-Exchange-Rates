using System.Net;
using System.Text;

namespace TCMB.Helper
{
    public class WebClientHelper
    {
        public static string Download(string url)
        {
            string result = null;

            try
            {
                result = new WebClient(){Encoding = Encoding.UTF8}.DownloadString(url);
            }
            catch
            {
                // ignored
            }

            return result;
        }
    }
}