using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers.Api;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/courses
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _context.Courses
            .Include(c => c.Teacher)
            .Select(c => new
            {
                c.ID,
                c.Name,
                c.Description,
                c.TeacherID,
                TeacherName = c.Teacher != null ? c.Teacher.Name : null
            })
            .ToListAsync();

        return Ok(courses);
    }

    // GET /api/courses/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Teacher)
            .Where(c => c.ID == id)
            .Select(c => new
            {
                c.ID,
                c.Name,
                c.Description,
                c.TeacherID,
                TeacherName = c.Teacher != null ? c.Teacher.Name : null
            })
            .FirstOrDefaultAsync();

        if (course == null)
            return NotFound(new { message = "Курс не знайдено" });

        return Ok(course);
    }
}