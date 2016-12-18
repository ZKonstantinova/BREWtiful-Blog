using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerBlog.Models;

namespace BeerBlog.Controllers.Admin
{
    public class SectionController : Controller
    {
        // GET: Section
        public ActionResult Index()
        {
            return RedirectToAction("ListCategories");
        }

        // GET: Section/List
        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var sections = database.Sections
                    .ToList();
              //  ViewBag.ListOfSections = new SelectList(sections);

                return View(sections);
            }
        }

        //
        // GET: Section/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: Section/Create
        [HttpPost]
        public ActionResult Create(Section section)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    database.Sections.Add(section);
                    database.SaveChanges();

                    return RedirectToAction("List");
                }
            }
            return View(section);
        }


        //
        // GET: Section/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            using (var database = new BlogDbContext())
            {
                var section = database.Sections
                    .FirstOrDefault(s => s.Id == id);

                if (section == null)
                {
                    return HttpNotFound();
                }

                return View(section);

            }
        }

        //
        // POST: Section/Edit
        [HttpPost]
        public ActionResult Edit(Section section)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    database.Entry(section).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(section);
        }

        // GET: Section/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var section = database.Sections
                    .FirstOrDefault(s => s.Id == id);

                if (section == null)
                {
                    return HttpNotFound();
                }

                return View(section);
            }
        }

        //
        // POST: Section/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var database = new BlogDbContext())
            {
                var section = database.Sections
                    .FirstOrDefault(s => s.Id == id);

                var sectionCategories = section.Categories
                    .ToList();

                foreach (var category in sectionCategories)
                {
                    database.Categories.Remove(category);
                }

                database.Sections.Remove(section);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult ListCategories()
        {
            using (var database = new BlogDbContext())
            {
                var categories = database.Categories
                    .Include(c => c.Articles)
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(categories);
            }
        }

        public ActionResult ListArticles(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var articles = database.Articles
                    .Where(a => a.CategoryId == categoryId)
                    .Include(a => a.Author)
                    .ToList();

                return View(articles);
            }
        }
    }
}