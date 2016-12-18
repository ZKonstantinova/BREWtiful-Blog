using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeerBlog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sections()
        {
          

            return RedirectToAction("List", "Category");
        }

        public ActionResult Places()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}