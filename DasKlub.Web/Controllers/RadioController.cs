using System.Web.Mvc;

namespace DasKlub.Web.Controllers
{
    public class RadioController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}