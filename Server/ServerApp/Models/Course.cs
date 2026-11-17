using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models;

public class Course
{
    public int ID { get; set; }
    [Required] public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? Semester { get; set; }
    public int TeacherID { get; set; }
    public Teacher? Teacher { get; set; }
}