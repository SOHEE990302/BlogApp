using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BlogApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly BlogDbContext _context;

        public AdminController(BlogDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var pendingUsers = _context.Users.Where(u => u.Role == "contributor").ToList();
            return View(pendingUsers);
        }

        [HttpPost]
        public IActionResult Approve(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.Role = "approved_contributor";
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }
    }
}