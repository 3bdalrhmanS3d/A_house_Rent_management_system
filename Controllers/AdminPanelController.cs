using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using System.Linq;
using System.Text.Json;
using Tasken2.DBContext;
using Tasken2.Models;
using Microsoft.EntityFrameworkCore;

namespace Tasken2.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly AppDbContext _context;

        public AdminPanelController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            string temp = HttpContext.Session.GetString("Message");
            HttpContext.Session.SetString("Message", "");
            ViewBag.Message = temp;

            var properties = _context.properties.AsQueryable();
            var users = _context.persons.Where(p => p.accountType == "user").AsQueryable();
            var admins = _context.persons.Where(p => p.accountType == "admin").AsQueryable();
            var areas = _context.Areas.AsQueryable();

            
            ViewBag.Approved = properties
                .Where(p => p.HireStatus == 1)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            ViewBag.Pending = properties
                .Where(p => p.HireStatus == 0)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            ViewBag.Area = areas
                .OrderByDescending(a => a.Id)
                .ToList();

            ViewBag.User = users
                .OrderByDescending(u => u.personID)
                .Select(u => new {
                    u.personID,
                    u.firstName,
                    u.lastName,
                    u.nationalID,
                    u.phoneNumber,
                    u.email,
                    u.accountType,
                    u.nationalIdImage
                })
                .ToList();

            ViewBag.Admin = admins
                .OrderByDescending(a => a.personID)
                .ToList();

            return View();
        }


        public IActionResult Search(int t, string searchString)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            string temp = HttpContext.Session.GetString("Message");
            HttpContext.Session.SetString("Message", "");
            ViewBag.Message = temp;

            var properties = _context.properties.AsQueryable();
            var users = _context.persons.Where(p => p.accountType == "user").AsQueryable();
            var admins = _context.persons.Where(p => p.accountType == "admin").AsQueryable();
            var areas = _context.Areas.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (t == 1)
                {
                    admins = admins.Where(a => a.firstName.Contains(searchString) || a.lastName.Contains(searchString) || a.email.Contains(searchString));
                }
                else
                {
                    users = users.Where(u => u.firstName.Contains(searchString) || u.lastName.Contains(searchString) || u.email.Contains(searchString));
                }
            }

            ViewBag.Approved = properties
                .Where(p => p.HireStatus == 1)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            ViewBag.Pending = properties
                .Where(p => p.HireStatus == 0)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            ViewBag.Area = areas
                .OrderByDescending(a => a.Id)
                .ToList();

            ViewBag.User = users
                .OrderByDescending(u => u.personID)
                .Select(u => new {
                    u.personID,
                    u.firstName,
                    u.lastName,
                    u.nationalID,
                    u.phoneNumber,
                    u.email,
                    u.accountType,
                    u.nationalIdImage
                })
                .ToList();

            ViewBag.Admin = admins
                .OrderByDescending(a => a.personID)
                .ToList();

            

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddComment(int propertyId, string commentText)
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));

            var comment = new Comments
            {
                commentText = commentText,
                propID = propertyId,
                personID = currentUser.personID,
                commentTime = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Comments.Add(comment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = propertyId });
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception or handle it as needed
                    Console.WriteLine($"An error occurred while saving the comment: {ex.Message}");
                    return BadRequest("An error occurred while saving the comment.");
                }
            }

            var property = await _context.properties
                .Include(p => p.CreatedBy)
                .Include(p => p.comments)
                    .ThenInclude(c => c.Person)
                .Include(p => p.PropertyRatings)
                    .ThenInclude(r => r.Person)
                .FirstOrDefaultAsync(p => p.propertyID == propertyId);

            if (property == null)
            {
                return NotFound();
            }

            // ترتيب التعليقات والتقييمات حسب الأحدث
            property.comments = property.comments.OrderByDescending(c => c.commentTime).ToList();
            property.PropertyRatings = property.PropertyRatings.OrderByDescending(r => r.CreatedAt).ToList();

            return View("Details", property);
        }


        public IActionResult DeletePost(int id)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var post = _context.properties.SingleOrDefault(p => p.propertyID == id);
            if (post != null)
            {
                _context.properties.Remove(post);
                _context.SaveChanges();
                HttpContext.Session.SetString("Message", "Post Deleted");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Report()
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var maxArea = _context.properties
                .Where(p => p.HireStatus == 0)
                .ToList()
                .GroupBy(p => p.AreaId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            ViewBag.AreaName = maxArea != null ? _context.Areas.SingleOrDefault(a => a.Id == maxArea.Key).AreaName : "N/A";

            var topKeyword = _context.searchHistories
                .ToList()
                .GroupBy(s => s.KeyWord)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            ViewBag.KeyWord = topKeyword?.Key ?? "N/A";

            var topIp = _context.searchHistories
                .ToList()
                .GroupBy(s => s.Ip)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            ViewBag.Ip = topIp?.Key ?? "N/A";

            return View();
        }

        public IActionResult ApprovePost(int id)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var post = _context.properties.SingleOrDefault(p => p.propertyID == id);
            if (post != null)
            {
                post.HireStatus = 1;
                _context.SaveChanges();
                HttpContext.Session.SetString("Message", "Post Approved");
            }

            return RedirectToAction("Index");
        }

        public IActionResult PendingPost(int id)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var post = _context.properties.SingleOrDefault(p => p.propertyID == id);
            if (post != null)
            {
                post.HireStatus = 0;
                _context.SaveChanges();
                HttpContext.Session.SetString("Message", "Post set to Pending");
            }

            return RedirectToAction("Index");
        }

        public IActionResult ApprovedPostCommentsCount()
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var approvedPostsCommentsCount = _context.properties
                .Where(p => p.HireStatus == 1)
                .Select(p => new
                {
                    p.propertyID,
                    CommentsCount = p.comments.Count
                })
                .ToList();

            ViewBag.ApprovedPostsCommentsCount = approvedPostsCommentsCount;

            return View();
        }


        [HttpGet]
        public IActionResult AddArea()
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddArea(Area area)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            _context.Areas.Add(area);
            _context.SaveChanges();
            HttpContext.Session.SetString("Message", "New Area Added");

            return RedirectToAction("Index");
        }

        public IActionResult DeleteArea(int id)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var area = _context.Areas.SingleOrDefault(a => a.Id == id);
            if (area != null)
            {
                _context.Areas.Remove(area);
                _context.SaveChanges();
                HttpContext.Session.SetString("Message", "Area Deleted");
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteUser(int id)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _context.persons.SingleOrDefault(p => p.personID == id);
            if (user != null)
            {
                var properties = _context.properties.Where(p => p.CreatedIDBy == id).ToList();
                var comments = _context.Comments.Where(c => c.personID == id).ToList();
                var ratings = _context.PropertyRatings.Where(r => r.personID == id).ToList();

                _context.properties.RemoveRange(properties);
                _context.Comments.RemoveRange(comments);
                _context.PropertyRatings.RemoveRange(ratings);
                _context.persons.Remove(user);
                _context.SaveChanges();

                HttpContext.Session.SetString("Message", "User and related data Deleted");
            }

            return RedirectToAction("Index");
        }

        public IActionResult MakeAdmin(int id)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _context.persons.SingleOrDefault(p => p.personID == id);
            if (user != null)
            {
                user.accountType = "admin";
                _context.SaveChanges();
                HttpContext.Session.SetString("Message", "New Admin Added");
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteAdmin(int id)
        {
            if (HttpContext.Session.GetString("Login") != "true" || HttpContext.Session.GetString("UserStatus") != "admin")
            {
                return RedirectToAction("Index", "Login");
            }

            var admin = _context.persons.SingleOrDefault(p => p.personID == id && p.accountType == "admin");
            if (admin != null)
            {
                admin.accountType = "user";
                _context.SaveChanges();
                HttpContext.Session.SetString("Message", "Admin Deleted");
            }

            return RedirectToAction("Index");
        }


    }
}