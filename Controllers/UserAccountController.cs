using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;
using Tasken2.DBContext;
using Tasken2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var person = JsonSerializer.Deserialize<Person>(HttpContext.Session.Get("CurrentLoginUser"));

            ViewBag.ActivePost = _context.properties.Where(p => p.CreatedIDBy == person.personID && p.HireStatus == 1).ToList();
            ViewBag.InActivePost = _context.properties.Where(p => p.CreatedIDBy == person.personID && p.HireStatus == 0).ToList();

            return View(person);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var person = JsonSerializer.Deserialize<Person>(HttpContext.Session.Get("CurrentLoginUser"));
            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(Person model)
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                var person = await _context.persons.FindAsync(model.personID);
                if (person != null)
                {
                    person.firstName = model.firstName;
                    person.lastName = model.lastName;
                    person.phoneNumber = model.phoneNumber;
                    person.password = model.password;
                    person.confirmPassword = model.confirmPassword;

                    await _context.SaveChangesAsync();
                    HttpContext.Session.Set("CurrentLoginUser", JsonSerializer.SerializeToUtf8Bytes(person));
                    TempData["message"] = "Profile updated successfully.";
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        /*[HttpPost]
        public IActionResult DisablePost(int id)
        {
            var property = _context.properties.Find(id);
            if (property != null)
            {
                property.HireStatus = 0;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ActivatePost(int id)
        {
            var property = _context.properties.Find(id);
            if (property != null)
            {
                property.HireStatus = 1;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var property = _context.properties.Find(id);
            if (property != null)
            {
                _context.properties.Remove(property);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }*/


        public IActionResult Account()
        {
            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));
            var activePosts = _context.properties.Where(p => p.CreatedIDBy == currentUser.personID && p.HireStatus == 1).ToList();
            var inactivePosts = _context.properties.Where(p => p.CreatedIDBy == currentUser.personID && p.HireStatus == 0).ToList();

            ViewBag.ActivePost = activePosts;
            ViewBag.InActivePost = inactivePosts;

            return View(currentUser);
        }


        public async Task<IActionResult> DisablePost(int id)
        {
            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));
            var property = _context.properties.FirstOrDefault(p => p.propertyID == id && p.CreatedIDBy == currentUser.personID);

            if (property == null)
            {
                return NotFound();
            }

            property.HireStatus = 0; // تعطيل الإعلان
            _context.properties.Update(property);
            await _context.SaveChangesAsync();
            TempData["message"] = "Post disabled successfully";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActivatePost(int id)
        {
            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));
            var property = _context.properties.FirstOrDefault(p => p.propertyID == id && p.CreatedIDBy == currentUser.personID);

            if (property == null)
            {
                return NotFound();
            }

            property.HireStatus = 1; // تفعيل الإعلان
            _context.properties.Update(property);
            await _context.SaveChangesAsync();
            TempData["message"] = "Post activated successfully";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeletePost(int id)
        {
            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));
            var property = _context.properties.FirstOrDefault(p => p.propertyID == id && p.CreatedIDBy == currentUser.personID);

            if (property == null)
            {
                return NotFound();
            }

            _context.properties.Remove(property);
            await _context.SaveChangesAsync();
            TempData["message"] = "Post deleted successfully";
            return RedirectToAction("Index");
        }


        // Get  PostDetail/EditProperty/3
        public async Task<IActionResult> EditProperty(int id)
        {

            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));
            var property = await _context.properties.FirstOrDefaultAsync(p => p.propertyID == id && p.CreatedIDBy == currentUser.personID);

            if (property == null)
            {
                return NotFound();
            }

            ViewBag.Arealist = GetAreaList();
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> EditProperty(Property prop)
        {

            if (HttpContext.Session.GetString("Login") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var currentUser = JsonSerializer.Deserialize<Person>(HttpContext.Session.GetString("CurrentLoginUser"));
            var property = _context.properties.FirstOrDefault(p => p.propertyID == prop.propertyID && p.CreatedIDBy == currentUser.personID);

            if (property == null)
            {
                return NotFound();
            }

            string[] fileNames = new string[5];
            for (int i = 1; i <= 5; i++)
            {
                var file = Request.Form.Files[$"FileUpload{i}"];

                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    var path = Path.Combine(_host.WebRootPath, "Images", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    fileNames[i - 1] = fileName;
                }
            }

            property.propPrice = prop.propPrice;
            property.probNumberOfRooms = prop.probNumberOfRooms;
            property.propArea = prop.propArea;
            property.propRegion = prop.propRegion;
            property.propStreet = prop.propStreet;
            property.propFloorNumber = prop.propFloorNumber;
            property.propImage1 = !string.IsNullOrEmpty(fileNames[0]) ? fileNames[0] : property.propImage1;
            property.propImage2 = !string.IsNullOrEmpty(fileNames[1]) ? fileNames[1] : property.propImage2;
            property.propImage3 = !string.IsNullOrEmpty(fileNames[2]) ? fileNames[2] : property.propImage3;
            property.propImage4 = !string.IsNullOrEmpty(fileNames[3]) ? fileNames[3] : property.propImage4;
            property.propImage5 = !string.IsNullOrEmpty(fileNames[4]) ? fileNames[4] : property.propImage5;
            property.AreaId = prop.AreaId;

            if (ModelState.IsValid)
            {
                _context.properties.Update(property);
                await _context.SaveChangesAsync();
                ViewBag.message = "Property updated successfully";
                return RedirectToAction("Index", "UserAccount");
            }
            else
            {
                ViewBag.message = "Property update was not successful";
                ViewBag.Arealist = GetAreaList();
                return View(prop);
            }
        }
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