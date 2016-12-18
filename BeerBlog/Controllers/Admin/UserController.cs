using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerBlog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace BeerBlog.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //
        // GET: User/List
        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var users = database.Users.ToList();

                var admins = GetAdminUserNames(users, database);
                ViewBag.Admins = admins;

                return View(users);
            }
            
        }

        private HashSet<string> GetAdminUserNames(List<ApplicationUser> users, BlogDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var admins = new HashSet<string>();

            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id, "Admin"))
                {
                    admins.Add(user.UserName);
                }

            }

            return admins;
        }

        //
        //GET: User/Edit
        public ActionResult Edit(string id)
        {
            // validate id
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // get user form db

                var user = database.Users
                    .Where(u => u.Id == id)
                    .First();
            
                // check if user exists
                if (user == null)
                {
                    return HttpNotFound();
                }

                // create view model
                var viewModel = new EditUserViewModel();
                viewModel.User = user;
                viewModel.Roles = GetUserRoles(user, database);

                // pass the model to the view
                return View(viewModel);
            }
        }

        private List<Role> GetUserRoles(ApplicationUser user, BlogDbContext db)
        {
            // create user manager
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();

            // get all appcilation roles
            var roles = db.Roles
                .Select(r => r.Name)
                .OrderBy(r => r)
                .ToList();

            // for each application role, check if user has it
            var userRoles = new List<Role>();

            foreach (var roleName in roles)
            {
                var role = new Role { Name = roleName };

                if (userManager.IsInRole(user.Id, roleName))
                {

                    role.IsSelected = true;
                }

                userRoles.Add(role);
            }

            // return list with all roles

            return userRoles;
        }

        //
        // POST: User/Edit
        [HttpPost]
        public ActionResult Edit(string id, EditUserViewModel viewModel)
        {
            // check if model is valid
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    // get user from db

                    var user = database.Users.FirstOrDefault(u => u.Id == id);

                    //check if user exists
                    if (user == null)
                    {
                        return HttpNotFound();
                    }

                    // if password field is not empty, change password

                    if (!string.IsNullOrEmpty(viewModel.Password))
                    {
                        var hasher = new PasswordHasher();
                        var passwordHash = hasher.HashPassword(viewModel.Password);
                        user.PasswordHash = passwordHash;
                    }
                    // set user properties
                    user.Email = viewModel.User.Email;
                    user.FullName = viewModel.User.FullName;
                    user.UserName = viewModel.User.Email;
                    this.SetUserRoles(viewModel, user, database);

                    // save changes
                    database.Entry(user).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("List");
                }

            }


            return View(viewModel);
        }

        private void SetUserRoles(EditUserViewModel model, ApplicationUser user, BlogDbContext db)
        {
            var userManager = Request
                .GetOwinContext()
                .GetUserManager<ApplicationUserManager>();


            foreach (var role in model.Roles)
            {
                if (role.IsSelected)
                {
                    userManager
                        .AddToRole(user.Id, role.Name);
                }
                else if (!role.IsSelected)
                {
                    userManager.RemoveFromRole(user.Id, role.Name);
                }
            }
        }

        //
        // GET: User/Delete
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            using (var database = new BlogDbContext())
            {
                // get user from db
                var user = database.Users
                    .Where(u => u.Id.Equals(id))
                    .First();
                    

                // check if user exists
                if (user == null)
                {
                    return HttpNotFound();

                }

                return View(user);
            }
        }

        //
        // POST: User/Delete
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                // get user from db
                var user = database.Users
                    .Where(u => u.Id.Equals(id))
                    .First();

                // get user articles from db

                var userArticles = database.Articles
                    .Where(a => a.Author.Id == user.Id);

                // delete user articles
                foreach (var article in userArticles)
                {
                    database.Articles.Remove(article);
                }

                // delete user and save changes
                database.Users.Remove(user);
                database.SaveChanges();

                return RedirectToAction("List");
            }
        }


    }
}