using System.Web.Mvc;

namespace DasKlub.Web.Controllers
{
    public class ErrorsController : Controller
    {
        //
        // GET: /Errors/

        public ActionResult Http404()
        {
            return View();
        }

        public ActionResult General()
        {
            return View();
        }
    }
}