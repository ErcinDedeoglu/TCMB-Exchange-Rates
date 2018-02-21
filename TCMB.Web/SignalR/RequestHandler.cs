using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using TCMB.Database.Adapter;
using TCMB.DTO.Database;
using TCMB.DTO.Rest;
using TCMB.Helper;

namespace TCMB.Web.SignalR
{
    public class RequestHandler
    {
        private static readonly CurrencyExchangeRateAdapter CurrencyExchangeRateAdapter = new CurrencyExchangeRateAdapter();
        public string ConnectionID { get; set; }
        
        public void ExchangeRates(string data)
        {
            DateTime date = DateTime.ParseExact(data.Replace("\"", ""), "dd.MM.yyyy", CultureInfo.InvariantCulture);

            IEnumerable<CurrencyExchangeRateDto> currencyExchangeRateList = new List<CurrencyExchangeRateDto>();

            try
            {
                currencyExchangeRateList = CurrencyExchangeRateAdapter.CurrencyExchangeRateList(date);

                if (!currencyExchangeRateList.Any())
                {
                    string xml = Helper.WebClientHelper.Download("http://www.tcmb.gov.tr/kurlar/" + date.ToString("yyyy") + date.ToString("MM") + "/" + date.ToString("dd") + date.ToString("MM") + date.ToString("yyyy") + ".xml");

                    if (!string.IsNullOrWhiteSpace(xml))
                    {
                        DTO.Rest.TcmbExchangeRateDto.Tarih_Date tcmbExchangeRate = Helper.XmlHelper.DeserializeObject<DTO.Rest.TcmbExchangeRateDto.Tarih_Date>(xml);

                        if (tcmbExchangeRate != null && tcmbExchangeRate.Currency.Any())
                        {
                            foreach (TcmbExchangeRateDto.Currency currency in tcmbExchangeRate.Currency)
                            {
                                CurrencyExchangeRateDto currencyExchangeRate = CurrencyExchangeRateAdapter.CurrencyExchangeRate(date, currency.CurrencyCode);

                                if (currencyExchangeRate == null)
                                {
                                    CurrencyExchangeRateAdapter.InsertCurrencyExchangeRate(new CurrencyExchangeRateDto()
                                    {
                                        CurrencyCode = currency.CurrencyCode,
                                        BanknoteBuying = currency.BanknoteBuying.ToDecimal(),
                                        BanknoteSelling = currency.BanknoteSelling.ToDecimal(),
                                        Unit = currency.Unit.ToInt(),
                                        CrossRateOther = currency.CrossRateOther.ToDecimal(),
                                        CrossRateUSD = currency.CrossRateUSD.ToDecimal(),
                                        CurrencyExchangeRateDate = date,
                                        CurrencyExchangeRateShortDate = Helper.DateHelper.ShortDate(date),
                                        ForexBuying = currency.ForexBuying.ToDecimal(),
                                        ForexSelling = currency.ForexSelling.ToDecimal()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            ResponseHandler.ExchangeRates(ConnectionID, JsonConvert.SerializeObject(currencyExchangeRateList));
        }
    }
}