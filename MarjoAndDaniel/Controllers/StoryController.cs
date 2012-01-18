using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarjoAndDaniel.Controllers
{
    public class StoryController : Controller
    {

        public ActionResult Index()
        {
            return View("HowWeMet");
        }

        public ActionResult HowWeMet()
        {
            return View("HowWeMet");
        }

        public ActionResult Engagement()
        {
            return View("Engagement");
        }

        public ActionResult Pictures()
        {
            return View("Pictures");
        }

    }
}
