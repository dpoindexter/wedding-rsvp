using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarjoAndDaniel.Controllers
{
    public class WeddingController : Controller
    {
        // GET: /Wedding/

        public ActionResult Index()
        {
            return View("Venue");
        }

        public ActionResult Venue()
        {
            return View("Venue");
        }

        public ActionResult Directions()
        {
            return Redirect("http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=12805+Arroyo+Doble+Drive,+Manchaca,+Texas&aq=&sll=30.12579,-97.823564&sspn=0.006867,0.013937&ie=UTF8&hq=&hnear=12805+Arroyo+Doble+Dr,+Manchaca,+Travis,+Texas+78652&ll=30.125401,-97.823349&spn=0.006867,0.013937&z=17");
        }

        public ActionResult Hotel()
        {
            return View("Hotel");
        }

        public ActionResult Registry()
        {
            return View("Registry");
        }

        public ActionResult WeddingParty()
        {
            return View("WeddingParty");
        }

    }
}
