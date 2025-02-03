using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly BlogDbContext _context;

        public ArticleController(BlogDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var articles = _context.Articles
                .Where(a => a.StartDate <= DateTime.UtcNow && (a.EndDate == null || a.EndDate >= DateTime.UtcNow))
                .OrderByDescending(a => a.CreateDate)
                .ToList();
            return View(articles);
        }

        public IActionResult Details(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.ContributorUsername = User.Identity?.Name ?? "Unknown";
                article.CreateDate = DateTime.UtcNow;
                _context.Articles.Add(article);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var username = User.Identity?.Name;
            var article = _context.Articles.FirstOrDefault(a => a.Id == id && a.ContributorUsername == username);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity?.Name;
                var existingArticle = _context.Articles.FirstOrDefault(a => a.Id == article.Id && a.ContributorUsername == username);
                if (existingArticle == null)
                {
                    return NotFound();
                }

                existingArticle.Title = article.Title;
                existingArticle.Body = article.Body;
                existingArticle.StartDate = article.StartDate;
                existingArticle.EndDate = article.EndDate;

                _context.Articles.Update(existingArticle);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var username = User.Identity?.Name;
            var article = _context.Articles.FirstOrDefault(a => a.Id == id && a.ContributorUsername == username);
            if (article != null)
            {
                _context.Articles.Remove(article);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
