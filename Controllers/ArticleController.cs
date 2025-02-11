using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using System;
using System.Linq;

namespace BlogApp.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly BlogDbContext _context;

        public ArticleController(BlogDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var articles = _context.Articles
                .Where(a => a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)
                .OrderByDescending(a => a.CreateDate)
                .ToList();

            return View(articles);
        }

        // 게시글 세부 페이지
        [AllowAnonymous] // 비로그인 사용자도 접근 가능
        public IActionResult Details(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // 게시글 작성 페이지
        [Authorize(Roles = "approved_contributor")] // 승인된 Contributor만 접근 가능
        public IActionResult Create()
        {
            return View();
        }

        // 게시글 작성 (POST)
        [HttpPost]
        [Authorize(Roles = "approved_contributor")]
        public IActionResult Create(Article model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreateDate = DateTime.UtcNow;
            model.ContributorUsername = HttpContext.User.Identity.Name; // 현재 로그인 사용자
            _context.Articles.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 게시글 수정 페이지
        [Authorize(Roles = "approved_contributor")]
        public IActionResult Edit(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == id);

            if (article == null || article.ContributorUsername != HttpContext.User.Identity.Name)
            {
                return Unauthorized(); // 본인 글이 아니면 접근 불가
            }

            return View(article);
        }

        // 게시글 수정 (POST)
        [HttpPost]
        [Authorize(Roles = "approved_contributor")]
        public IActionResult Edit(Article model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == model.ArticleId);

            if (article == null || article.ContributorUsername != HttpContext.User.Identity.Name)
            {
                return Unauthorized(); // 본인 글이 아니면 수정 불가
            }

            article.Title = model.Title;
            article.Body = model.Body;
            article.StartDate = model.StartDate;
            article.EndDate = model.EndDate;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // 게시글 삭제
        [Authorize(Roles = "approved_contributor")]
        public IActionResult Delete(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == id);

            if (article == null || article.ContributorUsername != HttpContext.User.Identity.Name)
            {
                return Unauthorized(); // 본인 글이 아니면 삭제 불가
            }

            _context.Articles.Remove(article);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
