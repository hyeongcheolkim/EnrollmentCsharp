using Enrollment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Enrollment.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Professor> Professors { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Enroll> Enrollments { get; set; }


  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Admin>().OwnsOne(e => e.MemberInfo);
        modelBuilder.Entity<Professor>().OwnsOne(e => e.MemberInfo);
        modelBuilder.Entity<Student>().OwnsOne(e => e.MemberInfo);
        
        modelBuilder.Entity<Course>().OwnsOne(e => e.CourseTime);
        
        modelBuilder.Entity<Classroom>()
            .HasIndex(e => e.Code) 
            .IsUnique();
        
        modelBuilder.Entity<Classroom>()
            .HasIndex(e => e.Name) 
            .IsUnique();

        modelBuilder.Entity<Department>()
            .HasIndex(e => e.Code) 
            .IsUnique();

        modelBuilder.Entity<Subject>()
            .HasIndex(e => e.Code)
            .IsUnique();
        
        modelBuilder.Entity<Subject>()
            .Property(e => e.Type)
            .HasConversion(new EnumToStringConverter<SubjectType>());

        modelBuilder.Entity<Enroll>()
            .Property(e => e.ScoreType)
            .HasConversion(new EnumToStringConverter<ScoreType>());
        
        modelBuilder.Entity<Course>()
            .OwnsOne(e => e.CourseTime)
            .Property(ct => ct.Day)
            .HasConversion(new EnumToStringConverter<Day>());
        
        modelBuilder.Entity<Course>()
            .HasMany(e => e.ProhibitedDepartments)
            .WithMany(); 
    }
}