using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Internship.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Internship.Controllers
{
    public class AccountController : Controller
    {
        private booksDBContext db;
        public AccountController(booksDBContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    IQueryable<Users> users = db.Users.Include(u => u.Login).Include(u => u.Password);
                    
                    var user = users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                    if (user != null)
                    {
                        await Authenticate(model.Login);

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Wrong login or password");
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            return View(model);

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IQueryable<Users> users = db.Users.Include(u => u.Login).Include(u => u.Password);
                Users user =  users.FirstOrDefault(u => u.Login == model.Login);
                if (user == null)
                {
                  
                    db.Users.Add(new Users { Login = model.Login, Password = model.Password });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Login); 

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "This user already exists");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
          
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
         
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
          
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
    
