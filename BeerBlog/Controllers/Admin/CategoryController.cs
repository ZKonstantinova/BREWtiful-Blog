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
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        //
        // GET: Category/List
        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var categories = database.Categories
                    .ToList();

                return View(categories);
            }
        }

        //
        // GET: Category/Create
        public ActionResult Create()
        {
            using (var database = new BlogDbContext())
           {
                var model = new CategoryEditModel();
                model.Sections = database.Sections
                    .OrderBy(s => s.Name)
                   .ToList();


                return View(model);
            }
                
        }

        //
        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(CategoryEditModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var category = new Category(model.Name, model.SectionId);
                    database.Categories.Add(category);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        //
        // GET: Category/Edit
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            using (var database = new BlogDbContext())
            {
                var category = database.Categories
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return HttpNotFound();
                }

                var model = new CategoryEditModel();
                model.Id = category.Id;
                model.Name = category.Name;
                model.SectionId = category.SectionId;
                model.Sections = database.Sections
                    .OrderBy(s => s.Name)
                    .ToList();
               

                return View(model);

            }
        }

        //
        // POST: Category/Edit
        [HttpPost]
        public ActionResult Edit(CategoryEditModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {

                    var category = database.Categories
                      .FirstOrDefault(c => c.Id == model.Id);

                    category.Name = model.Name;
                    category.SectionId = model.SectionId;
                  

                    database.Entry(category).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        // GET: Category/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var category = database.Categories
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }
        }

        //
        // POST: Category/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var database = new BlogDbContext())
            {
                var category = database.Categories
                    .FirstOrDefault(c => c.Id == id);

                var categoryArticles = category.Articles
                    .ToList();

                foreach (var article in categoryArticles)
                {
                    database.Articles.Remove(article);
                }

                database.Categories.Remove(category);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }

    }
}