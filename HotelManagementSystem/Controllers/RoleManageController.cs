using HotelManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HotelManagementSystem.Controllers
{
    public class RoleManageController : Controller
    {
        // GET: Admin_Roll

        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            List<Admin_RollCreation> rollList = new List<Admin_RollCreation>();
            if (HttpContext.User.IsInRole("Admin"))
            {
                foreach (var v in roleManager.Roles.ToList())
                {
                    rollList.Add(new Admin_RollCreation()
                    {
                        RoleName = v.Name,
                        Id = 0
                    });
                }
            }
            else
            {
                foreach (var v in roleManager.Roles.Where(w => w.Name != "Admin").ToList())
                {
                    rollList.Add(new Admin_RollCreation()
                    {
                        RoleName = v.Name,
                        Id = 0
                    });
                }
            }
            return View(rollList);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult RollCreate()
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult RollCreate(Admin_RollCreation admin_RollCreation)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            IdentityRole idenName = new IdentityRole();
            idenName.Name = admin_RollCreation.RoleName;
            IdentityResult r = roleManager.Create(idenName);
            if (r.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View();
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult UserList()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var userList = db.Users.ToList();
            return View(userList);
        }

        [Authorize]
        [HttpGet]
        public ActionResult UserProfile()
        {
            string id = HttpContext.User.Identity.GetUserId();
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser au1 = db.Users.Find(new object[] { id });            
            return View(au1);
        }
        [Authorize]
        public ActionResult UserDelete(string id)
        {

            if (string.IsNullOrEmpty(id))
            {

                id = HttpContext.User.Identity.GetUserId();
                ApplicationDbContext db1 = new ApplicationDbContext();

                ApplicationUser au1 = db1.Users.Find(new object[] { id });
                db1.Users.Remove(au1);
                db1.SaveChanges();

                return RedirectToAction("LogOff", "Account");
                //return RedirectToAction("Index", "Home");
            }

            ApplicationDbContext db = new ApplicationDbContext();

            ApplicationUser au = db.Users.Find(new object[] { id });
            db.Users.Remove(au);
            db.SaveChanges();

            return RedirectToAction("LogOff", "Account");
        }


        [HttpPost]
        [Authorize]
        public ActionResult Delete(string rollName)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {

                ApplicationDbContext db = new ApplicationDbContext();
                ApplicationDbContext dbRole = new ApplicationDbContext();

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbRole));
                var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(db));
                ICollection<IdentityUserRole> IdentityUserRoleList = roleManager.FindByName(rollName).Users;

                foreach (IdentityUserRole iur in IdentityUserRoleList)
                {
                    ApplicationUser au = db.Users.Find(new object[] { iur.UserId });
                    db.Users.Remove(au);
                    db.SaveChanges();

                }
                IdentityRole IdentRoleName = dbRole.Roles.Where(w => w.Name == rollName).FirstOrDefault();
                dbRole.Roles.Remove(IdentRoleName);
                dbRole.SaveChanges();
                return RedirectToAction("Index", "RoleManage");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }




        }
    }
}