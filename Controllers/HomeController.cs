using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogDbContext _context;

        public HomeController(BlogDbContext context)
        {
            _context = context;
        }

        // Home page with a list of articles
        [AllowAnonymous]  // Allow anonymous access (no login required)
        public IActionResult Index()
        {
            var articles = _context.Articles
                .Where(a => a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)  // Filter articles based on date
                .OrderByDescending(a => a.CreateDate)  // Sort articles by creation date
                .ToList();  // Convert to list

            return View(articles);  // Return the articles to the Index view
        }

        // Article details page
        [AllowAnonymous]  // Allow anonymous access (no login required)
        public IActionResult Details(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == id);

            if (article == null)
            {
                return NotFound();  // Return 404 if the article is not found
            }

            return View(article);  // Return the article to the Details view
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
