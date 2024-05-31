using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tasken2.DBContext;
using Tasken2.Models;

namespace Tasken2.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;
        public RegistrationController(AppDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
        public class Input
        {

            [Required]
            [StringLength(50)]
            public string firstName { get; set; } // الاسم الأول

            [Required]
            [StringLength(50)]
            public string lastName { get; set; } // الاسم الثاني

            [Required]
            [StringLength(14)]
            public string nationalID { get; set; } // رقم البطاقة الشخصية

            [Required]
            [StringLength(14)]
            [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "Invalid Phone Number")]
            public string phoneNumber { get; set; } // رقم الهاتف

            [Required]
            [EmailAddress]
            public string email { get; set; } // البريد الإلكتروني

            [Required]
            [DataType(DataType.Password)]
            public string password { get; set; } // كلمة المرور

            [Required]
            [DataType(DataType.Password)]
            [Compare("password", ErrorMessage = "Passwords does not match.")]
            public string confirmPassword { get; set; } // تأكيد كلمة المرور
            public IFormFile img_file { get; set; }


        }
        [BindProperty]
        public Input input { get; set; }
        // GET: /Registration/
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.message = "";
            return View();
        }

        // POST: /Registration/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexAsync()
        {
            Person person = new Person();


            if (input.img_file != null && input.img_file.Length > 0)
            {
                try
                {
                    // Get the wwwroot path
                    string uploadPath = Path.Combine(_host.WebRootPath, "images");
                    Directory.CreateDirectory(uploadPath);
                    // Generate a unique filename for the uploaded file
                    string newfilename = $"{Guid.NewGuid().ToString()}{Path.GetExtension(input.img_file.FileName)}";
                    string fileName = Path.Combine(uploadPath, newfilename);

                    // Save the file to the server
                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        input.img_file.CopyTo(fileStream);
                    }
                    person.nationalIdImage = Path.Combine("/images/", newfilename);
                    //TempData["ImagePath"] = Person.nationalIdImage;

                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading file: " + ex.Message);
                }
            }


            if (ModelState.IsValid)
            {
                var existingPerson = _context.persons.SingleOrDefault(x => x.email == person.email);
                if (existingPerson != null)
                {
                    ViewBag.message = "User already exists";


                    return RedirectToAction("Index", "Login");
                }

                person = new Person
                {
                    firstName = input.firstName,
                    lastName = input.lastName,
                    nationalID = input.nationalID,
                    phoneNumber = input.phoneNumber,
                    email = input.email,
                    password = input.password,
                    confirmPassword = input.confirmPassword,
                    nationalIdImage = input.nationalID,
                    accountType = "user",
                    createdAt = DateTime.Now
                };


                _context.persons.Add(person);
                _context.SaveChanges();
                ViewBag.message = "Registration successful";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ViewBag.message = "Registration fail";
                return View(person);
            }


        }
    }
}
