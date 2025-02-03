using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(BlogDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

[HttpPost]
public IActionResult Login(string username, string password)
{
    var user = _context.Users.FirstOrDefault(u => u.Username == username.Trim() && u.Password == password.Trim());

    if (user == null)
    {
        ViewBag.ErrorMessage = "Invalid username or password.";
        return View();
    }

    if (user.Role == "contributor")
    {
        ViewBag.ErrorMessage = "Your account requires administrator approval.";
        return View();
    }

    _httpContextAccessor.HttpContext!.Session.SetString("Username", user.Username);
    _httpContextAccessor.HttpContext.Session.SetString("Role", user.Role);

    return RedirectToAction("Index", "Home");
}


        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext!.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

[HttpPost]
public IActionResult Register(User model)
{
    if (!ModelState.IsValid)
    {
        return View(model); // 유효성 검사 실패 시 입력값과 오류 메시지를 그대로 유지
    }

    model.Role = "contributor"; // 기본 역할 설정
    _context.Users.Add(model);
    _context.SaveChanges();

    return RedirectToAction("PendingApproval");
}


        [HttpGet]
        public IActionResult PendingApproval()
        {
            return View();
        }
    }
}
