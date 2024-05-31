using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tasken2.Models;
using Tasken2.DBContext;

namespace Tasken2.Controllers
{

    public class PostDetailController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;

        public PostDetailController(AppDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var property = await _context.properties
                .Include(p => p.CreatedBy)
                .Include(p => p.comments)
                .ThenInclude(c => c.Person)
                .Include(p => p.PropertyRatings)
                .ThenInclude(r => r.Person)
                .FirstOrDefaultAsync(p => p.propertyID == id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        
    }
}
