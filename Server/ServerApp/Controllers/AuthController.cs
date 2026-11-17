using Microsoft.AspNetCore.Mvc;
using ServerApp.Services;

namespace ServerApp.Controllers;

public class AuthController : Controller
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (username == "admin" && password == "admin123")
        {
            var token = _jwtService.GenerateToken("0", "admin", "Admin");

            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(12)
            });

            return RedirectToAction("Index", "Teachers");
        }

        ViewBag.Error = "Неверный логин или пароль";
        return View();
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete("JwtToken");
        return RedirectToAction("Login");
    }
}