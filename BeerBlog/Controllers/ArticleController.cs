using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerBlog.Models;

namespace BeerBlog.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: Article/Details

            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // Get the article from the db
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();


            if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        //
        // GET: Article/List
        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                // GET articles from db
                var articles = database.Articles
                    .Include(a => a.Author)
                    .ToList();

                return View(articles);
            }
                
        }

        //
        // GET: Article Create
        [Authorize]
        public ActionResult Create()
        {
            using (var database = new BlogDbContext())
            {
                var model = new ArticleViewModel();
                model.Categories = database.Categories
                    .OrderBy(c => c.Name)
                    .ToList();

                return View(model);
            }
            
        }

        //
        // POST: Article/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    // get author id
                    var authorId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    // set articles author
                    var article = new Article(authorId, model.Title, model.Content, model.CategoryId);

                    // save article in db
                    database.Articles.Add(article);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        

        //
        // GET: Article/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // get article from db
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .Include(a => a.Category)
                    .First();

                if (!IsUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                // check if article exists
                if (article == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // pass article to view
                return View(article);
                
            }
        }

        //
        // POST: Article/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // get article from db
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .First();

                // check if article exists
                if (article == null)
                {
                    return HttpNotFound();
                }
                // delete article from db
                database.Articles.Remove(article);
                database.SaveChanges();

                // redirect to index page
                return RedirectToAction("Index");
            }
        }

        //
        // GET: Article/Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // get article from db
                var article = database.Articles
                    .Where(a => a.Id == id)
                    .First();

                if (!IsUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                // check if article exists
                if (article == null)
                {
                    return HttpNotFound();
                }
                // create the view model
                var model = new ArticleViewModel();
                model.Id = article.Id;
                model.Title = article.Title;
                model.Content = article.Content;
                model.CategoryId = article.CategoryId;
                model.Categories = database.Categories
                    .OrderBy(c => c.Name)
                    .ToList();

                // pass the view model to view
                return View(model);
            }
        }

        //
        // POST: Article/Edit
        [HttpPost]
        public ActionResult Edit(ArticleViewModel model)
        {
            // check if model state is valid
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    // get article from db
                    var article = database.Articles
                        .FirstOrDefault(a => a.Id == model.Id);

                    // set article properties
                    article.Title = model.Title;
                    article.Content = model.Content;
                    article.CategoryId = model.CategoryId;

                    // save article state in db
                    database.Entry(article).State = EntityState.Modified;
                    database.SaveChanges();

                    // redirect to index page
                    return RedirectToAction("Index");
                }
            }
            // if model state is invalid
            return View(model);
        }

        private bool IsUserAuthorizedToEdit(Article article)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = article.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}