using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;
using Tasken2.DBContext;
using Tasken2.Models;

namespace Tasken2.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Login/
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != null)
            {
                return RedirectToAction("Index", "UserHome");
            }
            ViewBag.Message = "";
            return View();
        }

        // POST: /Login/
        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            var person = _context.persons.SingleOrDefault(x => x.email == email && x.password == password);

            if (person != null)
            {
                // تحقق مما إذا كان المستخدم محظوراً
                var banUntilString = HttpContext.Session.GetString($"BanUntil_{person.email}");
                if (!string.IsNullOrEmpty(banUntilString))
                {
                    var banUntil = DateTime.Parse(banUntilString);
                    if (banUntil > DateTime.Now)
                    {
                        ViewBag.Message = $"Your account is banned until {banUntil}";
                        return View();
                    }
                }

                HttpContext.Session.SetString("Login", "true");
                HttpContext.Session.SetString("FullName", person.firstName + " " + person.lastName);
                HttpContext.Session.SetString("UserStatus", person.accountType);
                HttpContext.Session.Set("CurrentLoginUser", JsonSerializer.SerializeToUtf8Bytes(person));
                HttpContext.Session.SetString("Message", $"Welcome {person.firstName} {person.lastName}");

                if (person.accountType == "admin")
                {
                    return RedirectToAction("Index", "AdminPanel");
                }
                else
                {
                    return RedirectToAction("Index", "UserHome");
                }
            }
            else
            {
                ViewBag.Message = "Invalid Email or Password";
                return View();
            }
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
