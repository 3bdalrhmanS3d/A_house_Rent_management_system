using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tasken2.Controllers.DTOs;
using Tasken2.DBContext;
using Tasken2.Models;

namespace Tasken2.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;

        public UserAccountController(AppDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        private async Task<Person> GetCurrentUserAsync()
        {
            var id = HttpContext.Session.GetInt32("CurrentLoginUser");
            return id.HasValue
                ? await _context.Persons.FindAsync(id.Value)
                : null;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToAction("Index", "Login");

            var activePosts = await _context.Properties
                .Where(p => p.CreatedById == user.PersonID && p.HireStatus == 1)
                .ToListAsync();
            var inactivePosts = await _context.Properties
                .Where(p => p.CreatedById == user.PersonID && p.HireStatus == 0)
                .ToListAsync();

            var vm = new AccountViewModel
            {
                Person = user,
                ActivePosts = activePosts,
                InactivePosts = inactivePosts
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToAction("Index", "Login");

            var input = new ProfileInput
            {
                PersonId = user.PersonID,
                FullName = user.FullName,
                PhoneNumber = user.phoneNumber,
                Email = user.email
            };
            return View(input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ProfileInput input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var user = await _context.Persons.FindAsync(input.PersonId);
            if (user == null)
                return NotFound();

            user.FullName = input.FullName;
            user.phoneNumber = input.PhoneNumber;
            user.email = input.Email;

            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("FullName", user.FullName);
            TempData["message"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditProperty(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToAction("Index", "Login");

            var prop = await _context.Properties
                .FirstOrDefaultAsync(p => p.PropertyId == id && p.CreatedById == user.PersonID);
            if (prop == null)
                return NotFound();

            var input = new PropertyEditInput
            {
                PropertyId = prop.PropertyId,
                Price = prop.Price,
                Area = prop.Area,
                NumberOfRooms = prop.NumberOfRooms,
                Region = prop.Region,
                Street = prop.Street,
                FloorNumber = prop.FloorNumber,
                AreaId = prop.AreaId
            };
            ViewBag.AreaList = await _context.Areas
                .Select(a => new SelectListItem(a.AreaName, a.Id.ToString()))
                .ToListAsync();

            return View(input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(PropertyEditInput input)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AreaList = await _context.Areas
                    .Select(a => new SelectListItem(a.AreaName, a.Id.ToString()))
                    .ToListAsync();
                return View(input);
            }

            var user = await GetCurrentUserAsync();
            var prop = await _context.Properties
                .FirstOrDefaultAsync(p => p.PropertyId == input.PropertyId && p.CreatedById == user.PersonID);
            if (prop == null)
                return NotFound();

            if (input.ImageFiles != null)
            {
                for (int i = 0; i < input.ImageFiles.Length && i < 5; i++)
                {
                    var file = input.ImageFiles[i];
                    if (file?.Length > 0)
                    {
                        var filename = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        var path = Path.Combine(_host.WebRootPath, "images", filename);
                        using var stream = new FileStream(path, FileMode.Create);
                        await file.CopyToAsync(stream);

                        switch (i)
                        {
                            case 0: prop.Image1 = filename; break;
                            case 1: prop.Image2 = filename; break;
                            case 2: prop.Image3 = filename; break;
                            case 3: prop.Image4 = filename; break;
                            case 4: prop.Image5 = filename; break;
                        }
                    }
                }
            }

            prop.Price = input.Price;
            prop.Area = input.Area;
            prop.NumberOfRooms = input.NumberOfRooms;
            prop.Region = input.Region;
            prop.Street = input.Street;
            prop.FloorNumber = input.FloorNumber;
            prop.AreaId = input.AreaId;

            await _context.SaveChangesAsync();
            TempData["message"] = "Property updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DisablePost(int id)
        {
            var user = await GetCurrentUserAsync();
            var prop = await _context.Properties
                .FirstOrDefaultAsync(p => p.PropertyId == id && p.CreatedById == user.PersonID);
            if (prop == null) return NotFound();

            prop.HireStatus = 0;
            await _context.SaveChangesAsync();
            TempData["message"] = "Post disabled successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ActivatePost(int id)
        {
            var user = await GetCurrentUserAsync();
            var prop = await _context.Properties
                .FirstOrDefaultAsync(p => p.PropertyId == id && p.CreatedById == user.PersonID);
            if (prop == null) return NotFound();

            prop.HireStatus = 1;
            await _context.SaveChangesAsync();
            TempData["message"] = "Post activated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var user = await GetCurrentUserAsync();
            var prop = await _context.Properties
                .FirstOrDefaultAsync(p => p.PropertyId == id && p.CreatedById == user.PersonID);
            if (prop == null) return NotFound();

            _context.Properties.Remove(prop);
            await _context.SaveChangesAsync();
            TempData["message"] = "Post deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordInput model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await GetCurrentUserAsync();
            if (user == null)
                return RedirectToAction("Index", "Login");

            var hasher = HttpContext.RequestServices
                               .GetRequiredService<IPasswordHasher<Person>>();

            if (hasher.VerifyHashedPassword(user, user.PasswordHash, model.CurrentPassword)
                    == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                return View(model);
            }

            user.PasswordHash = hasher.HashPassword(user, model.NewPassword);
            await _context.SaveChangesAsync();

            TempData["message"] = "Password changed successfully.";
            return RedirectToAction(nameof(Index));
        }


        // POST: /UserAccount/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int propertyId, string commentText)
        {
            // إذا لم يكن مسجل دخول
            if (HttpContext.Session.GetString("Login") == null)
                return RedirectToAction("Index", "Login");

            // جلب بيانات المستخدم الحالي
            var user = HttpContext.Session.GetInt32("CurrentLoginUser");
            if (user == null)
                return RedirectToAction("Index", "Login");

            var comment = new Comments
            {
                PropertyId = propertyId,
                PersonId = user.Value,
                CommentText = commentText,
                CommentTime = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // إعادة عرض تفاصيل العقار (أو الصفحة التي تريده)
            return RedirectToAction("EditProperty", new { id = propertyId });
        }

        // POST: /UserAccount/AddRating
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRating(int propertyId, float rating)
        {
            if (HttpContext.Session.GetString("Login") == null)
                return RedirectToAction("Index", "Login");

            var user = HttpContext.Session.GetInt32("CurrentLoginUser");
            if (user == null)
                return RedirectToAction("Index", "Login");

            var propRating = new PropertyRating
            {
                PropertyId = propertyId,
                PersonId = user.Value,
                Rating = rating,
                CreatedAt = DateTime.UtcNow
            };

            _context.PropertyRatings.Add(propRating);
            await _context.SaveChangesAsync();

            return RedirectToAction("EditProperty", new { id = propertyId });
        }
    }
}