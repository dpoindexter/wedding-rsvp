using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Norm;
using PostmarkDotNet;
using MarjoAndDaniel.Models;
using MarjoAndDaniel.Data;

namespace MarjoAndDaniel.Controllers
{
    public class RsvpController : Controller
    {

        // GET: /Rsvp/
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        public ActionResult Update(int? id)
        {
            var model = new mongo().FindOne<Rsvp>(i => i.Id == id);
            var vm = new RsvpViewModel(model);
            return (vm != null) ? View("Index", vm) : View("Index");
        }

        [HttpPost]
        public ActionResult Index(RsvpViewModel vm)
        {
            try
            {
                if (vm.Id != null)
                {
                    Rsvp.Retrieve(vm).Save();
                    return View("ThankYou");
                }
                else
                {
                    ViewBag.Error = true;
                    return View("Index", vm);
                }
            }
            catch
            {
                ViewBag.Error = true;
                return View("Index", vm);
            }
        }

        public ActionResult ThankYou()
        {
            return View("ThankYou");
        }

        public ActionResult List()
        {
            var rsvp = new mongo().GetCollection<Rsvp>().OrderByDescending(i => i.RespondedOn);
            var vm = rsvp.Select(i => new RsvpViewModel(i));

            return View("List", vm);
        }

        public JsonResult Search(string term)
        {
            if (!String.IsNullOrEmpty(term))
            {
                var terms = term.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                var collection = new mongo().GetCollection<Rsvp>();
                var matches = collection.Where(col => col.SearchTerms.Intersect(terms, new TermComparer()).Count() > 0);

                return Json(matches.Select(i => new RsvpJsonModel(i)), JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Admin()
        {
            var models = new mongo().GetCollection<Rsvp>().Where(i => i.HasResponded).OrderByDescending(i => i.RespondedOn.GetValueOrDefault());
            return View("Admin", models);
        }

        public ActionResult NoResponse()
        {
            var models = new mongo().GetCollection<Rsvp>().Where(i => !i.HasResponded).OrderBy(i => i.Primary.Name);
            return View("NoResponse", models);
        }

        [HttpGet]
        public ActionResult Add(int? id)
        {
            RsvpViewModel vm;
            if (id != null)
            {
                var m = new mongo().GetItemById<Rsvp>(id);
                vm = new RsvpViewModel(m);
            }
            else
            {
                vm = new RsvpViewModel();
                vm.Primary.IsEditable = true;
                vm.PlusOne.IsEditable = true;
            }
            return View("Add", vm);
        }

        [HttpPost]
        public ActionResult Add(RsvpViewModel vm)
        {
            new Rsvp(vm).Create();
            return RedirectToAction("Add");
        }

        public ActionResult Dump()
        {
            var _db = new RsvpEntities();
            var seed = _db.RSVPs.Select(r =>
                new Rsvp
                {
                    Primary = new Guest { Name = r.Guest1Name, IsEditable = r.Guest1Editable ?? false },
                    PlusOne = new Guest { Name = r.Guest2Name, IsEditable = r.Guest2Editable ?? false },
                    IsAllowedPlusOne = r.AllowedPlusOne ?? false,
                    IsGuestPrefilled = r.GuestPrefilled ?? false
                }
            ).ToList();
            new mongo().Add<Rsvp>(seed);
            return View("Index");
        }
    }
}
