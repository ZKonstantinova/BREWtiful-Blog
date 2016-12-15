using System.Data.Entity;
using BeerBlog.Migrations;
using BeerBlog.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BeerBlog.Startup))]
namespace BeerBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<BlogDbContext, Configuration>());

            ConfigureAuth(app);
        }
    }
}
