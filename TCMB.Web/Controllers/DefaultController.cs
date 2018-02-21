using System.Web.Mvc;

namespace TCMB.Web.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            //string x = Helper.WebClientHelper.Download("http://www.tcmb.gov.tr/kurlar/201701/02012017.xml");
            //DTO.Rest.TcmbExchangeRateDto.Tarih_Date xx = Helper.XmlHelper.DeserializeObject<DTO.Rest.TcmbExchangeRateDto.Tarih_Date>(x);

            return View();
        }
    }
}