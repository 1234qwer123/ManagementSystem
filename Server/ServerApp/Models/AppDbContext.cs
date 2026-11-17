using Microsoft.EntityFrameworkCore;

namespace ServerApp.Models;

/// <summary>Контекст БД: Teachers (Users), Courses (Items), Proposals (Actions).</summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Proposal> Proposals => Set<Proposal>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Teacher>().HasIndex(t => t.Email).IsUnique();

        b.Entity<Course>().HasIndex(c => c.TeacherID);
        b.Entity<Course>().HasOne(c => c.Teacher).WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherID).OnDelete(DeleteBehavior.Restrict);

        b.Entity<Proposal>().HasIndex(p => p.TeacherID);
        b.Entity<Proposal>().HasOne(p => p.Teacher).WithMany(t => t.Proposals)
            .HasForeignKey(p => p.TeacherID).OnDelete(DeleteBehavior.Restrict);
    }
}