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
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Login") == "true"
                && HttpContext.Session.GetString("UserStatus") == "admin";
        }
        public IActionResult Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Index", "Login");

            ViewBag.Message = HttpContext.Session.GetString("Message");
            HttpContext.Session.Remove("Message");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ApprovedPosts()
        {
            if (!IsAdmin()) return Unauthorized();
            var approved = await _context.Properties
                .Where(p => p.HireStatus == 1)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return PartialView("_ApprovedPosts", approved);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAllPost()
        {
            var pending = await _context.Properties
                                .Where(p => p.HireStatus == 0)
                                .ToListAsync();

            foreach (var post in pending)
            {
                post.HireStatus = 1;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> PendingPosts()
        {
            if (!IsAdmin()) return Unauthorized();
            var pending = await _context.Properties
                .Where(p => p.HireStatus == 0)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return PartialView("_PendingPosts", pending);
        }

        [HttpGet]
        public async Task<IActionResult> Users(string search = null)
        {
            if (!IsAdmin()) return Unauthorized();
            var users = _context.Persons
                .Where(p => p.AccountType == "user");
            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.FullName.Contains(search) || u.email.Contains(search));
            }
            var list = await users.OrderByDescending(u => u.PersonID).ToListAsync();
            return PartialView("_Users", list);
        }

        [HttpGet]
        public async Task<IActionResult> Admins(string search = null)
        {
            if (!IsAdmin()) return Unauthorized();
            var admins = _context.Persons
                .Where(p => p.AccountType == "admin");
            if (!string.IsNullOrEmpty(search))
            {
                admins = admins.Where(a => a.FullName.Contains(search) || a.email.Contains(search));
            }
            var list = await admins.OrderByDescending(a => a.PersonID).ToListAsync();
            return PartialView("_Admins", list);
        }


        [HttpPost]
        public async Task<IActionResult> ApprovePost(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var post = await _context.Properties.FindAsync(id);
            if (post != null)
            {
                post.HireStatus = 1;
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Message", "Post Approved");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PendingPost(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var post = await _context.Properties.FindAsync(id);
            if (post != null)
            {
                post.HireStatus = 0;
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Message", "Post Set to Pending");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var post = await _context.Properties.FindAsync(id);
            if (post != null)
            {
                _context.Properties.Remove(post);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Message", "Post Deleted");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddArea()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
            return PartialView("_AddArea");
        }

        [HttpGet]
        public async Task<IActionResult> ListAreas()
        {
            if (!IsAdmin()) return Unauthorized();
            var areas = await _context.Areas.ToListAsync();
            return PartialView("_AreasList", areas);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddArea(Area area)
        {
            if (!IsAdmin()) return Unauthorized();
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("Message", "New Area Added");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArea(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var area = await _context.Areas.FindAsync(id);
            if (area != null)
            {
                _context.Areas.Remove(area);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Message", "Area Deleted");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var user = await _context.Persons.FindAsync(id);

            if (user != null)
            {
                var props = _context.Properties.Where(p => p.CreatedBy.PersonID == id);
                var comments = _context.Comments.Where(c => c.Person.PersonID == id);
                var ratings = _context.PropertyRatings.Where(r => r.Person.PersonID == id);
                _context.Properties.RemoveRange(props);
                _context.Comments.RemoveRange(comments);
                _context.PropertyRatings.RemoveRange(ratings);
                _context.Persons.Remove(user);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Message", "User and Data Deleted");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MakeAdmin(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var user = await _context.Persons.FindAsync(id);
            if (user != null)
            {
                user.AccountType = "admin";
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Message", "Admin Role Granted");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (!IsAdmin()) return Unauthorized();
            var admin = await _context.Persons.FindAsync(id);

            var currentUser = HttpContext.Session.GetInt32("CurrentLoginUser");
            if (admin.PersonID == currentUser)
            {
                return BadRequest();
            }

            if (admin != null && admin.AccountType == "admin")
            {
                admin.AccountType = "user";
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("Message", "Admin Role Removed");
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Report()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
            // Example report logic here
            ViewBag.ReportDate = System.DateTime.UtcNow;
            return PartialView("_Report");
        }

        [HttpGet]
        public IActionResult ApprovedPostsCommentsCount()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Login");
            var counts = _context.Properties
                .Where(p => p.HireStatus == 1)
                .Select(p => new { p.PropertyId, CommentsCount = p.Comments.Count })
                .ToList();
            return PartialView("_ApprovedPostsCommentsCount", counts);
        }

    }
}