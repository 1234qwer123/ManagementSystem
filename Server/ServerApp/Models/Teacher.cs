using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApp.Models;

public class Teacher
{
    public int ID { get; set; }
    [Required] public string Name { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";

    // Лише для форми адмінки, у БД не зберігається
    [NotMapped, Display(Name = "Пароль")] public string? Password { get; set; }

    public List<Course> Courses { get; set; } = new();
    public List<Proposal> Proposals { get; set; } = new();
}