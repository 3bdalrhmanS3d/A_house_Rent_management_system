using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tasken2.DBContext;
using Tasken2.Models;

namespace Tasken2.Controllers
{
    public class UserHomeController : Controller
    {
        private readonly AppDbContext _context;

        public UserHomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /UserHome/
        [HttpGet]
        public async Task<IActionResult> Index(string searchArea, decimal? searchPrice, float? searchAreaSize)
        {
            var query = _context.Properties
                .Include(p => p.CreatedBy)
                .Include(p => p.PropertyRatings)
                .Where(p => p.HireStatus == 1);

            if (!string.IsNullOrWhiteSpace(searchArea))
                query = query.Where(p => p.Region.Contains(searchArea));

            if (searchPrice.HasValue)
                query = query.Where(p => p.Price <= searchPrice.Value);

            if (searchAreaSize.HasValue)
                query = query.Where(p => p.Area <= searchAreaSize.Value);

            var properties = await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(properties);
        }

        // GET: /UserHome/Details/5
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

        // POST: /UserHome/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int propertyId, string commentText)
        {
            var userId = HttpContext.Session.GetInt32("CurrentLoginUser");
            if (!userId.HasValue)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrWhiteSpace(commentText))
                return RedirectToAction("Details", new { id = propertyId });

            var comment = new Comments
            {
                CommentText = commentText,
                PropertyId = propertyId,
                PersonId = userId.Value,
                CommentTime = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = propertyId });
        }

        // POST: /UserHome/AddRating
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRating(int propertyId, float rating)
        {
            var userId = HttpContext.Session.GetInt32("CurrentLoginUser");
            if (!userId.HasValue)
                return RedirectToAction("Index", "Login");

            var propertyRating = new PropertyRating
            {
                Rating = rating,
                PropertyId = propertyId,
                PersonId = userId.Value,
                CreatedAt = DateTime.UtcNow
            };

            _context.PropertyRatings.Add(propertyRating);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = propertyId });
        }
    }
}
