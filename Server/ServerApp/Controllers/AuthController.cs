using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ServerApp.Controllers;

public class AuthController : Controller
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (username?.Trim().ToLower() != "admin" || password?.Trim() != "admin123")
        {
            ViewBag.Error = "Невірний логін або пароль";
            return View();
        }

        var jwtKey = _config["Jwt:Key"] ?? "SuperSecretKeyForUniversityApp2026_AtLeast32Chars!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "admin"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        Response.Cookies.Append("JwtToken", tokenString, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddHours(12)
        });

        return RedirectToAction("Index", "Teachers");
    }

    public IActionResult Logout()
    {
        Response.Cookies.Delete("JwtToken");
        return RedirectToAction("Login", "Auth");
    }
}
