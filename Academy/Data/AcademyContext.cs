using Microsoft.EntityFrameworkCore;
using Academy.Models;

public class AcademyContext(DbContextOptions<AcademyContext> options) : DbContext(options)
{
    public DbSet<Direction> Directions { get; set; } = default!;
    public DbSet<Group> Groups { get; set; } = default!;
    public DbSet<Student> Students { get; set; } = default!;
    public DbSet<Teacher> Teachers { get; set; } = default!;
    public DbSet<Discipline> Disciplines { get; set; } = default!;
    public DbSet<TeacherDiscipline> TeacherDisciplines { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Настройка составного ключа для связи Многие-ко-Многим (Teacher <-> Discipline)
        modelBuilder.Entity<TeacherDiscipline>()
            .HasKey(td => new { td.teacher_id, td.discipline_id });

        modelBuilder.Entity<TeacherDiscipline>()
            .HasOne(td => td.Teacher)
            .WithMany(t => t.TeacherDisciplines)
            .HasForeignKey(td => td.teacher_id);

        modelBuilder.Entity<TeacherDiscipline>()
            .HasOne(td => td.Discipline)
            .WithMany(d => d.TeacherDisciplines)
            .HasForeignKey(td => td.discipline_id);
    }
}
