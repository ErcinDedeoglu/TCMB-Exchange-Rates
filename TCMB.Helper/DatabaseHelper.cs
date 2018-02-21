using System.Data.SqlClient;

namespace TCMB.Helper
{
    public class DatabaseHelper
    {
        public static string BuildConnectionString(string dataSource = null, bool? integratedSecurity = null, string initialCatalog = null, string userID = null, string password = null)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            if (dataSource != null)
            {
                builder.DataSource = dataSource;
            }

            if (integratedSecurity != null)
            {
                builder.IntegratedSecurity = (bool)integratedSecurity;
            }

            if (initialCatalog != null)
            {
                builder.InitialCatalog = initialCatalog;
            }

            if (userID != null)
            {
                builder.UserID = userID;
            }

            if (password != null)
            {
                builder.Password = password;
            }

            return builder.ConnectionString;
        }
    }
}