using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers.Api;

[ApiController]
[Route("api/proposals")]
[Authorize]
public class ProposalsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProposalsApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/proposals
    [HttpGet]
    public async Task<IActionResult> GetProposals()
    {
        var proposals = await _context.Proposals
            .Include(p => p.Teacher)
            .Select(p => new
            {
                p.ID,
                p.TeacherID,
                TeacherName = p.Teacher != null ? p.Teacher.Name : null,
                p.CourseName,
                p.ProposalType,
                p.Status
            })
            .ToListAsync();

        return Ok(proposals);
    }

    // POST /api/proposals
    [HttpPost]
    public async Task<IActionResult> CreateProposal([FromBody] CreateProposalRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (request.ProposalType != "Create" && request.ProposalType != "Delete")
            return BadRequest(new { message = "ProposalType має бути 'Create' або 'Delete'" });

        var teacherExists = await _context.Teachers.AnyAsync(t => t.ID == request.TeacherID);
        if (!teacherExists)
            return NotFound(new { message = $"Викладача з ID={request.TeacherID} не знайдено" });

        var proposal = new Proposal
        {
            TeacherID = request.TeacherID,
            CourseName = request.CourseName.Trim(),
            ProposalType = request.ProposalType,
            Status = "Pending"
        };

        _context.Proposals.Add(proposal);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            proposal.ID,
            proposal.TeacherID,
            proposal.CourseName,
            proposal.ProposalType,
            proposal.Status
        });
    }
}

public class CreateProposalRequest
{
    [Required(ErrorMessage = "Вкажіть TeacherID")]
    [Range(1, int.MaxValue, ErrorMessage = "Некоректний TeacherID")]
    public int TeacherID { get; set; }

    [Required(ErrorMessage = "Назва курсу обов'язкова")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Назва курсу має містити від 2 до 150 символів")]
    public string CourseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Тип пропозиції обов'язковий")]
    public string ProposalType { get; set; } = "Create";
}