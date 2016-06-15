using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Невірний логін або пароль.");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    ApplicationContext userscontext = new ApplicationContext();
                    var userStore = new UserStore<ApplicationUser>(userscontext);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    var roleStore = new RoleStore<IdentityRole>(userscontext);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    if (!roleManager.RoleExists("Admin"))
                    {
                        roleManager.Create(new IdentityRole("Admin"));
                    }

                    if (!roleManager.RoleExists("Student"))
                    {
                        roleManager.Create(new IdentityRole("Student"));
                    }

                    if (!roleManager.RoleExists("Teacher"))
                    {
                        roleManager.Create(new IdentityRole("Teacher"));
                    }

                    if (!userManager.IsInRole("7ceba0d4-09a5-4adc-b090-97a020022cfe", "Admin"))
                    {
                        userManager.AddToRole("7ceba0d4-09a5-4adc-b090-97a020022cfe", "Admin");
                    }

                    if (userManager.IsInRole(user.Id, "Admin") == false && userManager.IsInRole(user.Id, "Student") == false
                        && userManager.IsInRole(user.Id, "Teacher"))
                    {
                        userManager.AddToRole(user.Id, "Student");
                    }

                    if (String.IsNullOrEmpty(returnUrl) && UserManager.IsInRole(user.Id, "Student"))
                    {
                        return RedirectToAction("List", "Themes");
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(returnUrl) && UserManager.IsInRole(user.Id, "Admin"))
                        {
                            return RedirectToAction("Index", "Roles");
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(returnUrl) && UserManager.IsInRole(user.Id, "Teacher"))
                            {
                                return RedirectToAction("List", "Article");
                            }
                        }
                    }
                    return RedirectToAction("List", "Article");
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}