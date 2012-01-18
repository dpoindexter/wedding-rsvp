using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PostmarkDotNet;

namespace MarjoAndDaniel.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/

        public ActionResult Index()
        {
            return View("Index", new PostmarkMessage());
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Referrer = (HttpContext.Request.UrlReferrer != null)
                ? HttpContext.Request.UrlReferrer.ToString()
                : "";

            return View("Contact");
        }

        [HttpPost]
        public ActionResult Contact(PostmarkMessage msg)
        {
            msg.From = "communications@regstep.com";
            msg.To = "dp.texas@gmail.com,marjo.manalang@gmail.com";

            return View("ThankYou");

            /*
            try
            {
                PostmarkClient client = new PostmarkClient("d5e6b4a6-5c98-4e63-a9ce-c22e6d5cdafc");
                PostmarkResponse response = client.SendMessage(msg);

                return View("ThankYou");
            }
            catch
            {
                return View("Contact", msg);
            }
            */
        }

        public ActionResult ThankYou()
        {
            return View("ThankYou");
        }

    }
}
