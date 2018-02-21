using System;

namespace TCMB.DTO.Database
{
    public class CurrencyExchangeRateDto
    {
        public int CurrencyExchangeRateID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? BanknoteBuying { get; set; }
        public decimal? BanknoteSelling { get; set; }
        public decimal? ForexBuying { get; set; }
        public decimal? ForexSelling { get; set; }
        public decimal? CrossRateUSD { get; set; }
        public decimal? CrossRateOther { get; set; }
        public int? Unit { get; set; }
        public DateTime CurrencyExchangeRateDate { get; set; }
        public long CurrencyExchangeRateShortDate { get; set; }
    }
}