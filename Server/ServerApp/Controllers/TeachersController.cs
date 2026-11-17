using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[Authorize(Roles = "Admin")]
public class TeachersController : Controller
{
    private readonly AppDbContext _context;

    public TeachersController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var teachers = await _context.Teachers.ToListAsync();
        return View(teachers);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Teacher teacher)
    {
        if (!string.IsNullOrWhiteSpace(teacher.PasswordHash))
        {
            teacher.PasswordHash = BCrypt.Net.BCrypt.HashPassword(teacher.PasswordHash);
        }

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();
        return View(teacher);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Teacher teacher)
    {
        var existing = await _context.Teachers.FindAsync(teacher.ID);
        if (existing == null) return NotFound();

        existing.Name = teacher.Name;
        existing.Email = teacher.Email;

        if (!string.IsNullOrWhiteSpace(teacher.PasswordHash))
        {
            existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(teacher.PasswordHash);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();
        return View(teacher);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher != null)
        {
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
