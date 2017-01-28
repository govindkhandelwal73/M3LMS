using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M3LMS.Controllers
{
    [HandleError]
    public class TutorialController : Controller
    {
        //
        // GET: /Tutorial/

        public ActionResult Index()
        {
            return View();
        }

    }
}
