using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[Authorize(Roles = "Admin")]
public class ProposalsController : Controller
{
    private readonly AppDbContext _context;

    public ProposalsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var proposals = await _context.Proposals.Include(p => p.Teacher).ToListAsync();
        return View(proposals);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "ID", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Proposal proposal)
    {
        _context.Proposals.Add(proposal);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var proposal = await _context.Proposals.FindAsync(id);
        if (proposal == null) return NotFound();

        ViewBag.Teachers = new SelectList(await _context.Teachers.ToListAsync(), "ID", "Name", proposal.TeacherID);
        return View(proposal);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Proposal proposal)
    {
        _context.Proposals.Update(proposal);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var proposal = await _context.Proposals.Include(p => p.Teacher).FirstOrDefaultAsync(p => p.ID == id);
        if (proposal == null) return NotFound();
        return View(proposal);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var proposal = await _context.Proposals.FindAsync(id);
        if (proposal != null)
        {
            _context.Proposals.Remove(proposal);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
