using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasken2.Controllers.DTOs;
using Tasken2.DBContext;
using Tasken2.Models;

namespace Tasken2.Controllers
{
    public class PropertyController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;

        public PropertyController(AppDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        // GET: /Property/Add
        [HttpGet]
        public IActionResult Add()
        {
            if (HttpContext.Session.GetString("Login") == null)
                return RedirectToAction("Index", "Login");

            ViewBag.AreaList = GetAreaList();
            return View();
        }

        // POST: /Property/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PropertyInput input)
        {
            if (HttpContext.Session.GetString("Login") == null)
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                ViewBag.AreaList = GetAreaList();
                return View(input);
            }

            var userId = HttpContext.Session.GetInt32("CurrentLoginUser");
            if (!userId.HasValue)
                return RedirectToAction("Index", "Login");

            var prop = new Property
            {
                Price = input.propPrice,
                Area = input.propArea,
                NumberOfRooms = input.probNumberOfRooms,
                Region = input.propRegion,
                Street = input.propStreet,
                FloorNumber = input.propFloorNumber,
                CreatedAt = DateTime.UtcNow,
                CreatedById = userId.Value,
                AreaId = input.AreaId
            };

            var uploadPath = Path.Combine(_host.WebRootPath, "images");
            Directory.CreateDirectory(uploadPath);

            var files = new IFormFile[] { input.propImage1File, input.propImage2File, input.propImage3File, input.propImage4File, input.propImage5File };
            for (int i = 0; i < files.Length; i++)
            {
                var file = files[i];
                if (file != null && file.Length > 0)
                {
                    var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadPath, filename);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                    var relative = "/images/" + filename;
                    switch (i)
                    {
                        case 0: prop.Image1 = relative; break;
                        case 1: prop.Image2 = relative; break;
                        case 2: prop.Image3 = relative; break;
                        case 3: prop.Image4 = relative; break;
                        case 4: prop.Image5 = relative; break;
                    }
                }
            }

            _context.Properties.Add(prop);
            await _context.SaveChangesAsync();

            TempData["message"] = "Ad posted successfully.";
            return RedirectToAction("Index", "UserAccount");
        }

        // GET: /Property/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var property = await _context.Properties
                .Include(p => p.CreatedBy)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Person)
                .Include(p => p.PropertyRatings)
                    .ThenInclude(r => r.Person)
                .FirstOrDefaultAsync(p => p.PropertyId == id);

            if (property == null)
                return NotFound();

            property.Comments = property.Comments
                .OrderByDescending(c => c.CommentTime)
                .ToList();
            property.PropertyRatings = property.PropertyRatings
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            return View(property);
        }

        private List<SelectListItem> GetAreaList()
        {
            return _context.Areas
                .Select(a => new SelectListItem(a.AreaName, a.Id.ToString()))
                .ToList();
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SeedProperties()
        {
            
            var seedData = new[]
            {
                new {
                    Price          = 250000m,
                    Area           = 150.5f,
                    NumberOfRooms  = 3,
                    Region         = "Region A",
                    Street         = "Street 1",
                    FloorNumber    = 2,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1A.jpg",
                    Image2         = "image2A.jpg",
                    Image3         = "image3A.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 3,
                    AreaId         = 1,
                    HireStatus     = 0
                },
                new {
                    Price          = 300000m,
                    Area           = 175.0f,
                    NumberOfRooms  = 4,
                    Region         = "Region B",
                    Street         = "Street 2",
                    FloorNumber    = 5,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1B.jpg",
                    Image2         = "image2B.jpg",
                    Image3         = "image3B.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 4,
                    AreaId         = 2,
                    HireStatus     = 0
                },
                new {
                    Price          = 150000m,
                    Area           = 100.0f,
                    NumberOfRooms  = 2,
                    Region         = "Region C",
                    Street         = "Street 3",
                    FloorNumber    = 1,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1C.jpg",
                    Image2         = "image2C.jpg",
                    Image3         = "image3C.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 5,
                    AreaId         = 3,
                    HireStatus     = 0
                },
                new {
                    Price          = 450000m,
                    Area           = 200.0f,
                    NumberOfRooms  = 5,
                    Region         = "Region D",
                    Street         = "Street 4",
                    FloorNumber    = 3,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1D.jpg",
                    Image2         = "image2D.jpg",
                    Image3         = "image3D.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 6,
                    AreaId         = 4,
                    HireStatus     = 0
                },
                new {
                    Price          = 350000m,
                    Area           = 180.0f,
                    NumberOfRooms  = 4,
                    Region         = "Region E",
                    Street         = "Street 5",
                    FloorNumber    = 4,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1E.jpg",
                    Image2         = "image2E.jpg",
                    Image3         = "image3E.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 7,
                    AreaId         = 5,
                    HireStatus     = 0
                },
                new {
                    Price          = 275000m,
                    Area           = 160.0f,
                    NumberOfRooms  = 3,
                    Region         = "Region F",
                    Street         = "Street 6",
                    FloorNumber    = 6,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1F.jpg",
                    Image2         = "image2F.jpg",
                    Image3         = "image3F.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 8,
                    AreaId         = 6,
                    HireStatus     = 0
                },
                new {
                    Price          = 220000m,
                    Area           = 140.0f,
                    NumberOfRooms  = 3,
                    Region         = "Region G",
                    Street         = "Street 7",
                    FloorNumber    = 2,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1G.jpg",
                    Image2         = "image2G.jpg",
                    Image3         = "image3G.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 9,
                    AreaId         = 7,
                    HireStatus     = 0
                },
                new {
                    Price          = 320000m,
                    Area           = 170.0f,
                    NumberOfRooms  = 4,
                    Region         = "Region H",
                    Street         = "Street 8",
                    FloorNumber    = 3,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1H.jpg",
                    Image2         = "image2H.jpg",
                    Image3         = "image3H.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 10,
                    AreaId         = 8,
                    HireStatus     = 0
                },
                new {
                    Price          = 275000m,
                    Area           = 165.0f,
                    NumberOfRooms  = 3,
                    Region         = "Region I",
                    Street         = "Street 9",
                    FloorNumber    = 2,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1I.jpg",
                    Image2         = "image2I.jpg",
                    Image3         = "image3I.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 11,
                    AreaId         = 9,
                    HireStatus     = 0
                },
                new {
                    Price          = 330000m,
                    Area           = 185.0f,
                    NumberOfRooms  = 4,
                    Region         = "Region J",
                    Street         = "Street 10",
                    FloorNumber    = 3,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1J.jpg",
                    Image2         = "image2J.jpg",
                    Image3         = "image3J.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 12,
                    AreaId         = 10,
                    HireStatus     = 0
                },
                new {
                    Price          = 310000m,
                    Area           = 175.0f,
                    NumberOfRooms  = 4,
                    Region         = "Region K",
                    Street         = "Street 11",
                    FloorNumber    = 2,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1K.jpg",
                    Image2         = "image2K.jpg",
                    Image3         = "image3K.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 13,
                    AreaId         = 11,
                    HireStatus     = 0
                },
                new {
                    Price          = 290000m,
                    Area           = 160.0f,
                    NumberOfRooms  = 3,
                    Region         = "Region L",
                    Street         = "Street 12",
                    FloorNumber    = 4,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1L.jpg",
                    Image2         = "image2L.jpg",
                    Image3         = "image3L.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 14,
                    AreaId         = 12,
                    HireStatus     = 0
                },
                new {
                    Price          = 280000m,
                    Area           = 155.0f,
                    NumberOfRooms  = 3,
                    Region         = "Region M",
                    Street         = "Street 13",
                    FloorNumber    = 2,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1M.jpg",
                    Image2         = "image2M.jpg",
                    Image3         = "image3M.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 15,
                    AreaId         = 13,
                    HireStatus     = 0
                },
                new {
                    Price          = 270000m,
                    Area           = 150.0f,
                    NumberOfRooms  = 2,
                    Region         = "Region N",
                    Street         = "Street 14",
                    FloorNumber    = 1,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1N.jpg",
                    Image2         = "image2N.jpg",
                    Image3         = "image3N.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 16,
                    AreaId         = 14,
                    HireStatus     = 0
                },
                new {
                    Price          = 240000m,
                    Area           = 130.0f,
                    NumberOfRooms  = 2,
                    Region         = "Region O",
                    Street         = "Street 15",
                    FloorNumber    = 3,
                    CreatedAt      = DateTime.Parse("2024-05-25 00:00:00"),
                    Image1         = "image1O.jpg",
                    Image2         = "image2O.jpg",
                    Image3         = "image3O.jpg",
                    Image4         = (string?)null,
                    Image5         = (string?)null,
                    CreatedById    = 17,
                    AreaId         = 15,
                    HireStatus     = 0
                }
            };

            foreach (var s in seedData)
            {
                bool exists = _context.Properties.Any(p =>
                    p.Region == s.Region &&
                    p.Street == s.Street &&
                    p.FloorNumber == s.FloorNumber &&
                    p.CreatedById == s.CreatedById);
                if (exists) continue;

                var p = new Property
                {
                    Price = s.Price,
                    Area = s.Area,
                    NumberOfRooms = s.NumberOfRooms,
                    Region = s.Region,
                    Street = s.Street,
                    FloorNumber = s.FloorNumber,
                    CreatedAt = s.CreatedAt,
                    Image1 = s.Image1,
                    Image2 = s.Image2,
                    Image3 = s.Image3,
                    Image4 = s.Image4,
                    Image5 = s.Image5,
                    CreatedById = s.CreatedById,
                    AreaId = s.AreaId,
                    HireStatus = s.HireStatus
                };

                _context.Properties.Add(p);
            }

            await _context.SaveChangesAsync();
            return Content("✅ Seeded properties successfully.");
        }
    }
}
