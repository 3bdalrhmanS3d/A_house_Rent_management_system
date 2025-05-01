using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tasken2.Controllers.DTOs;
using Tasken2.DBContext;
using Tasken2.Migrations;
using Tasken2.Models;

namespace Tasken2.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly IPasswordHasher<Person> _passwordHasher;

        public RegistrationController(AppDbContext context,
                                      IWebHostEnvironment host,
                                      IPasswordHasher<Person> passwordHasher)
        {
            _context = context;
            _host = host;
            _passwordHasher = passwordHasher;
        }

        // GET: /Registration/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Registration/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(PersonInput input)
        {
            if (!ModelState.IsValid)
                return View(input);

            bool exists = await _context.Persons
                                 .AnyAsync(u => u.email == input.email);
            if (exists)
            {
                ModelState.AddModelError(string.Empty, "User already exists.");
                return View(input);
            }

            var person = new Person
            {
                FullName = $"{input.FirstName} {input.LastName}",
                nationalID = input.nationalID,
                phoneNumber = input.phoneNumber,
                email = input.email,
                PasswordHash = _passwordHasher.HashPassword(null, input.password),
                AccountType = "user",
                CreatedAt = DateTime.UtcNow
            };

            if (input.NationalIdImageFile != null && input.NationalIdImageFile.Length > 0)
            {
                string uploads = Path.Combine(_host.WebRootPath, "images");
                Directory.CreateDirectory(uploads);
                string filename = Guid.NewGuid() + Path.GetExtension(input.NationalIdImageFile.FileName);
                string filePath = Path.Combine(uploads, filename);
                using var stream = new FileStream(filePath, FileMode.Create);
                await input.NationalIdImageFile.CopyToAsync(stream);
                person.NationalIdImage = "/images/" + filename;
            }

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Login");
        }

        // GET: Display the "Forgot Password" form
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordViewModel());
        }

        // POST: Handle the "Forgot Password" submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Find user by National ID
            var user = await _context.Persons
                                     .SingleOrDefaultAsync(x => x.nationalID == vm.NationalID);

            if (user == null)
            {
                ModelState.AddModelError("", "No user was found with that National ID.");
                return View(vm);
            }

            // Generate a one-hour reset token
            var resetToken = Guid.NewGuid().ToString();

            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            };

            // Store token and user email in cookies
            Response.Cookies.Append("PWResetToken", resetToken, cookieOptions);
            Response.Cookies.Append("PWResetEmail", user.email, cookieOptions);

            TempData["Info"] = "A password reset link is valid for 1 hour.";
            return RedirectToAction("ResetPassword");
        }

        // GET: Display the "Reset Password" form
        [HttpGet]
        public IActionResult ResetPassword()
        {
            // If no valid reset token, redirect back
            if (!Request.Cookies.ContainsKey("PWResetToken"))
            {
                TempData["Error"] = "The reset link has expired or is invalid.";
                return RedirectToAction("ForgetPassword");
            }

            return View(new ResetPasswordViewModel());
        }

        // POST: Handle the "Reset Password" submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            // Verify token and email exist in cookies
            if (!Request.Cookies.TryGetValue("PWResetToken", out var token) ||
                !Request.Cookies.TryGetValue("PWResetEmail", out var email))
            {
                TempData["Error"] = "The reset link has expired or is invalid.";
                return RedirectToAction("ForgetPassword");
            }

            if (!ModelState.IsValid)
                return View(vm);

            // Find the user by email
            var user = await _context.Persons
                                     .SingleOrDefaultAsync(x => x.email == email);
            if (user == null)
                return BadRequest("Unexpected error: user not found.");

            // Hash and save the new password
            user.PasswordHash = _passwordHasher.HashPassword(user, vm.NewPassword);
            await _context.SaveChangesAsync();

            // Remove the reset cookies
            Response.Cookies.Delete("PWResetToken");
            Response.Cookies.Delete("PWResetEmail");

            TempData["Success"] = "Your password has been reset successfully. You can now log in.";
            return RedirectToAction("Index", "Login");
        }
        
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)] //to hide this action from the Swagger/Explorer
        public async Task<IActionResult> SeedUsers()
        {
            //raw data
            var seedData = new[]
            {
                new { First = "John",    Last = "Doe",        NationalID = "12345678901234", Phone = "12345678901", Email = "john.doe@example.com",        Password = "password123" },
                new { First = "Jane",    Last = "Smith",      NationalID = "23456789012345", Phone = "23456789012", Email = "jane.smith@example.com",      Password = "password123" },
                new { First = "Michael", Last = "Johnson",    NationalID = "34567890123456", Phone = "34567890123", Email = "michael.johnson@example.com",Password = "password123" },
                new { First = "Alice",   Last = "Williams",   NationalID = "45678901234567", Phone = "45678901234", Email = "alice.williams@example.com", Password = "password123" },
                new { First = "Bob",     Last = "Brown",      NationalID = "56789012345678", Phone = "56789012345", Email = "bob.brown@example.com",       Password = "password123" },
                new { First = "Carol",   Last = "Davis",      NationalID = "67890123456789", Phone = "67890123456", Email = "carol.davis@example.com",     Password = "password123" },
                new { First = "David",   Last = "Miller",     NationalID = "78901234567890", Phone = "78901234567", Email = "david.miller@example.com",    Password = "password123" },
                new { First = "Eve",     Last = "Wilson",     NationalID = "89012345678901", Phone = "89012345678", Email = "eve.wilson@example.com",      Password = "password123" },
                new { First = "Frank",   Last = "Moore",      NationalID = "90123456789012", Phone = "90123456789", Email = "frank.moore@example.com",     Password = "password123" },
                new { First = "Grace",   Last = "Taylor",     NationalID = "12345678901234", Phone = "12345678901", Email = "grace.taylor@example.com",   Password = "password123" },
                new { First = "Hank",    Last = "Anderson",   NationalID = "23456789012345", Phone = "23456789012", Email = "hank.anderson@example.com", Password = "password123" },
                new { First = "Ivy",     Last = "Thomas",     NationalID = "34567890123456", Phone = "34567890123", Email = "ivy.thomas@example.com",     Password = "password123" },
                new { First = "Jack",    Last = "Jackson",    NationalID = "45678901234567", Phone = "45678901234", Email = "jack.jackson@example.com",   Password = "password123" },
                new { First = "Kathy",   Last = "White",      NationalID = "56789012345678", Phone = "56789012345", Email = "kathy.white@example.com",  Password = "password123" },
                new { First = "Leo",     Last = "Harris",     NationalID = "67890123456789", Phone = "67890123456", Email = "leo.harris@example.com",    Password = "password123" },
                new { First = "Mia",     Last = "Martin",     NationalID = "78901234567890", Phone = "78901234567", Email = "mia.martin@example.com",    Password = "password123" },
                new { First = "Nina",    Last = "Thompson",   NationalID = "89012345678901", Phone = "89012345678", Email = "nina.thompson@example.com",Password = "password123" },
                new { First = "Oscar",   Last = "Garcia",     NationalID = "90123456789012", Phone = "90123456789", Email = "oscar.garcia@example.com",  Password = "password123" },
                new { First = "Pam",     Last = "Martinez",   NationalID = "12345678901234", Phone = "12345678901", Email = "pam.martinez@example.com", Password = "password123" },
                new { First = "Quinn",   Last = "Robinson",   NationalID = "23456789012345", Phone = "23456789012", Email = "quinn.robinson@example.com",Password = "password123" },
                new { First = "Rita",    Last = "Clark",      NationalID = "34567890123456", Phone = "34567890123", Email = "rita.clark@example.com",    Password = "password123" },
                new { First = "Steve",   Last = "Rodriguez",  NationalID = "45678901234567", Phone = "45678901234", Email = "steve.rodriguez@example.com",Password = "password123" },
                new { First = "Tina",    Last = "Lewis",      NationalID = "56789012345678", Phone = "56789012345", Email = "tina.lewis@example.com",    Password = "password123" }
            };

            foreach (var u in seedData)
            {
                if (await _context.Persons.AnyAsync(p => p.email == u.Email))
                    continue;

                var person = new Person
                {
                    FullName = $"{u.First} {u.Last}",
                    nationalID = u.NationalID,
                    phoneNumber = u.Phone,
                    email = u.Email,
                    PasswordHash = _passwordHasher.HashPassword(null, u.Password),
                    AccountType = "User",
                    CreatedAt = DateTime.UtcNow
                };

                _context.Persons.Add(person);
            }

            await _context.SaveChangesAsync();
            return Content("✅ The default users were created successfully.");
        }

    }
}
