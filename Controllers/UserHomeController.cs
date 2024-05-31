using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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

        public async Task<IActionResult> Index(string searchArea, decimal? searchPrice, float? searchAreaSize)
        {
            var properties = _context.properties
                .Include(p => p.CreatedBy)
                .Include(p => p.PropertyRatings)
                .Where(p => p.HireStatus == 1);

            // إضافة شروط البحث
            if (!string.IsNullOrEmpty(searchArea))
            {
                properties = properties.Where(p => p.propRegion.Contains(searchArea));
            }

            if (searchPrice.HasValue)
            {
                properties = properties.Where(p => p.propPrice <= searchPrice);
            }

            if (searchAreaSize.HasValue)
            {
                properties = properties.Where(p => p.propArea <= searchAreaSize);
            }

            var result = await properties
                .OrderByDescending(p => p.CreatedAt) // ترتيب حسب الأحدث
                .ToListAsync();

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var property = await _context.properties
                .Include(p => p.CreatedBy)
                .Include(p => p.comments)
                    .ThenInclude(c => c.Person)
                .Include(p => p.PropertyRatings)
                    .ThenInclude(r => r.Person)
                .FirstOrDefaultAsync(m => m.propertyID == id);

            if (property == null)
            {
                return NotFound();
            }

            // ترتيب التعليقات والتقييمات حسب الأحدث
            property.comments = property.comments.OrderByDescending(c => c.commentTime).ToList();
            property.PropertyRatings = property.PropertyRatings.OrderByDescending(r => r.CreatedAt).ToList();

            return View(property);
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

        [HttpPost]
        public async Task<IActionResult> AddRating(int propertyId, float rating)
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));

            var propertyRating = new PropertyRating
            {
                Rating = rating,
                propID = propertyId,
                personID = currentUser.personID,
                CreatedAt = DateTime.Now
            };

            if (ModelState.IsValid)
            {
                _context.PropertyRatings.Add(propertyRating);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = propertyId });
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
    }
}
