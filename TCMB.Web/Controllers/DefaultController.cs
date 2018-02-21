using System.Web.Mvc;

namespace TCMB.Web.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}