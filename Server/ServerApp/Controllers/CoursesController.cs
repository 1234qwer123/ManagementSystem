using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[Authorize(Roles = "Admin")]
public class CoursesController : Controller
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses.Include(c => c.Teacher).ToListAsync();
        return View(courses);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "ID", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Course course)
    {
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "ID", "Name", course.TeacherID);
        return View(course);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var course = await _context.Courses.Include(c => c.Teacher).FirstOrDefaultAsync(c => c.ID == id);
        if (course == null) return NotFound();
        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
