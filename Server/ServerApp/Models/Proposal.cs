using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models;

public class Proposal
{
    public int ID { get; set; }
    public int TeacherID { get; set; }
    public Teacher? Teacher { get; set; }
    [Required] public string CourseName { get; set; } = "";
    [Required] public string ProposalType { get; set; } = "Create"; // Create / Delete
    public string Status { get; set; } = "Pending";
}