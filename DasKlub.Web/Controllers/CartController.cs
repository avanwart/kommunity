using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DasKlub.Web.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConfirmOrder()
        {
            return new EmptyResult();
        }

        public ActionResult Receipt()
        {
            return new EmptyResult();
        }
    }
}
