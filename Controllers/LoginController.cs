using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tasken2.Controllers.DTOs;
using Tasken2.DBContext;
using Tasken2.Models;
using System;
using System.Threading.Tasks;

namespace Tasken2.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Person> _passwordHasher;

        public LoginController(AppDbContext context, IPasswordHasher<Person> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Auto-login via cookies
            if (HttpContext.Session.GetString("Login") == null
                && Request.Cookies.TryGetValue("RememberMeEmail", out var cookieEmail)
                && Request.Cookies.TryGetValue("RememberMePassword", out var cookiePwd))
            {
                var person = await _context.Persons.SingleOrDefaultAsync(u => u.email == cookieEmail);
                if (person != null &&
                    _passwordHasher.VerifyHashedPassword(person, person.PasswordHash, cookiePwd)
                        != PasswordVerificationResult.Failed)
                {
                    HttpContext.Session.SetString("Login", "true");
                    HttpContext.Session.SetString("FullName", person.FullName);
                    HttpContext.Session.SetString("UserStatus", person.AccountType);
                    HttpContext.Session.SetInt32("CurrentLoginUser", person.PersonID);
                    return RedirectToAction("Index", "UserHome");
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginInput input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var person = await _context.Persons.SingleOrDefaultAsync(u => u.email == input.Email);
            if (person == null ||
                _passwordHasher.VerifyHashedPassword(person, person.PasswordHash, input.Password)
                    == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(input);
            }

            HttpContext.Session.SetString("Login", "true");
            HttpContext.Session.SetString("FullName", person.FullName);
            HttpContext.Session.SetString("UserStatus", person.AccountType);
            HttpContext.Session.SetInt32("CurrentLoginUser", person.PersonID);

            if (input.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                };
                Response.Cookies.Append("RememberMeEmail", input.Email, cookieOptions);
                Response.Cookies.Append("RememberMePassword", input.Password, cookieOptions);
            }

            return RedirectToAction("Index", "UserHome");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("RememberMeEmail");
            Response.Cookies.Delete("RememberMePassword");
            return RedirectToAction("Index", "Home");
        }
    }
}
