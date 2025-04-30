using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tasken2.Controllers.DTOs;
using Tasken2.DBContext;
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
