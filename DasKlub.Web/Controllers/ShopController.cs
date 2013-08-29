using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PayPal.AdaptivePayments;

namespace DasKlub.Web.Controllers
{
    public class ShopController : Controller
    {
        //
        // GET: /Shop/

        public ActionResult Index()
        {
            PayPal.AdaptivePayments.AdaptivePaymentsService dd = new AdaptivePaymentsService();
            
            return View();
        }

        public ActionResult Item(int itemID, string key)
        {
            return View();
        }

    }
}
