using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using DasKlub.Lib.Operational;
using log4net;

namespace DasKlub.Web.Controllers
{
    public class DonationController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Donation/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SubmitDonation()
        {
            return View();
        }

        public ActionResult DonationConfirmation()
        {
            return View();
        }

        public ActionResult DonationError()
        {
            return View();
        }

        public ActionResult Notify(NameValueCollection nvc)
        {
          Utilities.LogError("ipn test");
          Utilities.LogError(nvc.ToString());
            return View();
        }

        public ActionResult Notify(   )
        {
            Utilities.LogError("ipn test1");
            Utilities.LogError(Request.Form.ToString());
            
            return View();
        }
    }
}
