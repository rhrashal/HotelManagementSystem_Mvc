using HotelManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HotelManagementSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            IdentityRole idenName = new IdentityRole();
            idenName.Name = "Admin";
            IdentityResult r = roleManager.Create(idenName);
            if (r.Succeeded)
            {
                var roleManager2 = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                IdentityRole idenName2 = new IdentityRole();
                idenName2.Name = "User";
                IdentityResult s = roleManager2.Create(idenName2);
                if (s.Succeeded)
                {
                     createDefaultRollAndUser();
                }

                          
            }

            
        }


        public async void createDefaultRollAndUser()
        {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                var user = new ApplicationUser { UserName = "admin@gmail.com", Email = "admin@gmail.com" };
                var result = await UserManager.CreateAsync(user, "admin123"); //password=admin123
                if (result.Succeeded)
                {
                    var userIdInstance = UserManager.Users.Where(w => w.UserName == "admin@gmail.com").FirstOrDefault();

                    UserManager.AddToRole(userIdInstance.Id, "Admin");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
