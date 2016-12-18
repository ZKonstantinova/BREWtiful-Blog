using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeerBlog.Models;

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
          

            return View();
        }

        public ActionResult Places()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

      
    }
}