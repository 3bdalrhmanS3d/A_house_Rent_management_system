using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Tasken2.DBContext;
using Tasken2.Models;
using static Tasken2.Controllers.RegistrationController;
using System;

namespace Tasken2.Controllers
{
    public class PostpropertyAddController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;

        public PostpropertyAddController(AppDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public class Input
        {
            [Required]
            [Column(TypeName = "money")]
            public decimal propPrice { get; set; }

            [Required]
            public float propArea { get; set; }

            [Required]
            public int probNumberOfRooms { get; set; }

            [Required]
            [StringLength(50)]
            public string propRegion { get; set; }
            [Required]
            [StringLength(50)]
            public string propStreet { get; set; }
            [Required]
            public int propFloorNumber { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;

            [NotMapped]
            public List<IFormFile> clientFiles { get; set; } = new List<IFormFile>();

            [NotMapped]
            public IFormFile propImage1File { get; set; }

            [NotMapped]
            public IFormFile propImage2File { get; set; }

            [NotMapped]
            public IFormFile propImage3File { get; set; }

            [NotMapped]
            public IFormFile propImage4File { get; set; }

            [NotMapped]
            public IFormFile propImage5File { get; set; }
            public string propImage1 { get; set; }
            public string propImage2 { get; set; }
            public string propImage3 { get; set; }
            public string? propImage4 { get; set; }
            public string? propImage5 { get; set; }
            public int CreatedIDBy { get; set; } 
            public int AreaId { get; set; }
            public int HireStatus { get; set; } = 0;
        }

        [BindProperty]
        public Input input { get; set; }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Arealist = GetAreaList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Property prop, FileMode propImage1File , FileMode propImage2File , FileMode propImage3File, FileMode propImage4File , FileMode propImage5File)
        {

            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            Property newprop = new Property();
            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));

            try
            {
                string uploadPath = Path.Combine(_host.WebRootPath, "images");
                Directory.CreateDirectory(uploadPath);

                if (input.propImage1File != null && input.propImage1File.Length > 0)
                {
                    string newfilename = $"{Guid.NewGuid()}{Path.GetExtension(input.propImage1File.FileName)}";
                    string fileName = Path.Combine(uploadPath, newfilename);

                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await input.propImage1File.CopyToAsync(fileStream);
                    }

                    newprop.propImage1 = Path.Combine("/images/", newfilename);
                }

                if (input.propImage2File != null && input.propImage2File.Length > 0)
                {
                    string newfilename = $"{Guid.NewGuid()}{Path.GetExtension(input.propImage2File.FileName)}";
                    string fileName = Path.Combine(uploadPath, newfilename);

                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await input.propImage2File.CopyToAsync(fileStream);
                    }

                    newprop.propImage2 = Path.Combine("/images/", newfilename);
                }

                if (input.propImage3File != null && input.propImage3File.Length > 0)
                {
                    string newfilename = $"{Guid.NewGuid()}{Path.GetExtension(input.propImage3File.FileName)}";
                    string fileName = Path.Combine(uploadPath, newfilename);

                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await input.propImage3File.CopyToAsync(fileStream);
                    }

                    newprop.propImage3 = Path.Combine("/images/", newfilename);
                }

                if (input.propImage4File != null && input.propImage4File.Length > 0)
                {
                    string newfilename = $"{Guid.NewGuid()}{Path.GetExtension(input.propImage4File.FileName)}";
                    string fileName = Path.Combine(uploadPath, newfilename);

                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await input.propImage4File.CopyToAsync(fileStream);
                    }

                    newprop.propImage4 = Path.Combine("/images/", newfilename);
                }

                if (input.propImage5File != null && input.propImage5File.Length > 0)
                {
                    string newfilename = $"{Guid.NewGuid()}{Path.GetExtension(input.propImage5File.FileName)}";
                    string fileName = Path.Combine(uploadPath, newfilename);

                    using (var fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        await input.propImage5File.CopyToAsync(fileStream);
                    }

                    newprop.propImage5 = Path.Combine("/images/", newfilename);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading file: " + ex.Message);
            }

            newprop = new Property
            {
                propPrice = prop.propPrice,
                probNumberOfRooms = prop.probNumberOfRooms,
                propArea = prop.propArea,
                propRegion = prop.propRegion,
                propStreet = prop.propStreet,
                propFloorNumber = prop.propFloorNumber,
                CreatedAt = DateTime.Now,
                /*propImage1 = input.propImage1,
                propImage2 = input.propImage2,
                propImage3 = input.propImage3,
                propImage4 = input.propImage4,
                propImage5 = input.propImage5,*/
                CreatedIDBy = currentUser.personID, // تعيين معرف المستخدم الحالي
                AreaId = prop.AreaId
            };

            if (ModelState.IsValid)
            {
                _context.properties.Add(newprop);
                await _context.SaveChangesAsync();
                ViewBag.message = "Posted Ad successfully";
                return RedirectToAction("Index", "UserAccount");
            }
            else
            {
                ViewBag.message = "Post Ad was not successful";
                ViewBag.Arealist = GetAreaList();
                return View(prop);
            }
        }

        /*public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));
            var property = _context.properties.FirstOrDefault(p => p.propertyID == id && p.CreatedIDBy == currentUser.personID);

            if (property == null)
            {
                return NotFound();
            }

            ViewBag.Arealist = GetAreaList();
            return View(property);
        }*/

        private List<SelectListItem> GetAreaList()
        {
            return _context.Areas.Select(a => new SelectListItem
            {
                Text = a.AreaName,
                Value = a.Id.ToString()
            }).ToList();
        }

        

    }
}
