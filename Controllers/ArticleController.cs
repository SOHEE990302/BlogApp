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
        [Authorize(Roles = "approved_contributor,admin")] // 승인된 Contributor와 관리자만 접근 가능
        public IActionResult Create()
        {
            return View();
        }

        // 게시글 작성 (POST)
        [HttpPost]
        [Authorize(Roles = "approved_contributor,admin")]
        public IActionResult Create(Article model)
        {

            // if (!ModelState.IsValid)
            // {
            //     return View(model);
            // }


            model.CreateDate = DateTime.UtcNow;

            // Ensure ContributorUsername is correctly set
            model.ContributorUsername = HttpContext.User.Identity.Name;

            // Double-check if model binding is overwriting the ContributorUsername (check if it’s already populated)
            if (string.IsNullOrEmpty(model.ContributorUsername))
            {
                Console.WriteLine("ContributorUsername is empty, setting it manually.");
                model.ContributorUsername = HttpContext.User.Identity.Name; // Set the current user
            }

            _context.Articles.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Redirect after successful creation
        }




        // 게시글 수정 페이지
        [Authorize(Roles = "approved_contributor,admin")]
        public IActionResult Edit(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == id);

            if (article == null || article.ContributorUsername != HttpContext.User.Identity.Name && !User.IsInRole("admin"))
            {
                return Unauthorized(); // 본인 글이 아니면 접근 불가
            }

            return View(article);
        }

        // 게시글 수정 (POST)
        [HttpPost]
        [Authorize(Roles = "approved_contributor,admin")]
        public IActionResult Edit(Article model)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors
            }

            // Log the values to verify
            Console.WriteLine($"ArticleId: {model.ArticleId}, Title: {model.Title}, Body: {model.Body}");

            // Find the article by its ArticleId
            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == model.ArticleId);

            if (article == null || article.ContributorUsername != HttpContext.User.Identity.Name && !User.IsInRole("admin"))
            {
                return Unauthorized(); // Check if the article exists and if the current user is the author
            }

            // Update the article's properties
            article.Title = model.Title;
            article.Body = model.Body;
            article.StartDate = model.StartDate;
            article.EndDate = model.EndDate;

            // Save changes to the database
            _context.SaveChanges();

            return RedirectToAction("Index"); // Redirect to the Index page after successful edit
        }




        // 게시글 삭제
        [Authorize(Roles = "approved_contributor,admin")]
        public IActionResult Delete(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.ArticleId == id);

            if (article == null || article.ContributorUsername != HttpContext.User.Identity.Name && !User.IsInRole("admin"))
            {
                return Unauthorized(); // 본인 글이 아니면 삭제 불가
            }

            _context.Articles.Remove(article);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}