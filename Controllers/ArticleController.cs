using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;

namespace BlogApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly BlogDbContext _context;

        public ArticleController(BlogDbContext context)
        {
            _context = context;
        }

        // GET: /Article
        public IActionResult Index()
        {
            var articles = _context.Articles
                .Where(a => a.StartDate == null || a.StartDate <= DateTime.UtcNow)
                .Where(a => a.EndDate == null || a.EndDate >= DateTime.UtcNow)
                .OrderByDescending(a => a.CreateDate)
                .ToList();

            return View(articles);
        }

        // GET: /Article/Details/{id}
        public IActionResult Details(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: /Article/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Article/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.ContributorUsername = User.Identity.Name; // 현재 로그인한 사용자 이름 설정
                article.CreateDate = DateTime.UtcNow;

                _context.Articles.Add(article);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        // GET: /Article/Edit/{id}
        [Authorize]
        public IActionResult Edit(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);

            if (article == null || article.ContributorUsername != User.Identity.Name)
            {
                return Unauthorized(); // 권한이 없는 경우
            }

            return View(article);
        }

        // POST: /Article/Edit/{id}
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Article updatedArticle)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);

            if (article == null || article.ContributorUsername != User.Identity.Name)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                article.Title = updatedArticle.Title;
                article.Body = updatedArticle.Body;
                article.StartDate = updatedArticle.StartDate;
                article.EndDate = updatedArticle.EndDate;

                _context.Articles.Update(article);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(updatedArticle);
        }

        // GET: /Article/Delete/{id}
        [Authorize]
        public IActionResult Delete(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);

            if (article == null || article.ContributorUsername != User.Identity.Name)
            {
                return Unauthorized();
            }

            return View(article);
        }

        // POST: /Article/Delete/{id}
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);

            if (article == null || article.ContributorUsername != User.Identity.Name)
            {
                return Unauthorized();
            }

            _context.Articles.Remove(article);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}