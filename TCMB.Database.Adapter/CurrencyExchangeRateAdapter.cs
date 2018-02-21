using System;
using System.Collections.Generic;
using TCMB.DTO.Database;

namespace TCMB.Database.Adapter
{
    public class CurrencyExchangeRateAdapter
    {
        public DTO.Database.CurrencyExchangeRateDto InsertCurrencyExchangeRate(DTO.Database.CurrencyExchangeRateDto currencyExchangeRate)
        {
            using (NPoco.IDatabase dbContext = new NPoco.Database(DTO.Global.SQLConnectionString, NPoco.DatabaseType.SqlServer2012))
            {
                dbContext.Insert("tbl_CurrencyExchangeRate", "CurrencyExchangeRateID", currencyExchangeRate);
            }

            return currencyExchangeRate;
        }

        public DTO.Database.CurrencyExchangeRateDto CurrencyExchangeRate(DateTime date, string currencyCode)
        {
            DTO.Database.CurrencyExchangeRateDto result;

            using (NPoco.IDatabase dbContext = new NPoco.Database(DTO.Global.SQLConnectionString, NPoco.DatabaseType.SqlServer2012))
            {
                result = dbContext.FirstOrDefault<DTO.Database.CurrencyExchangeRateDto>("SELECT * FROM tbl_CurrencyExchangeRate WHERE CurrencyExchangeRateShortDate = " + Helper.DateHelper.ShortDate(date) + " AND CurrencyCode = '" + currencyCode + "'");
            }

            return result;
        }

        public IEnumerable<CurrencyExchangeRateDto> CurrencyExchangeRateList(DateTime date)
        {
            IEnumerable<CurrencyExchangeRateDto> result;

            using (NPoco.IDatabase dbContext = new NPoco.Database(DTO.Global.SQLConnectionString, NPoco.DatabaseType.SqlServer2012))
            {
                result = dbContext.Query<DTO.Database.CurrencyExchangeRateDto>("SELECT * FROM tbl_CurrencyExchangeRate WHERE CurrencyExchangeRateShortDate = " + Helper.DateHelper.ShortDate(date));
            }

            return result;
        }
    }
}