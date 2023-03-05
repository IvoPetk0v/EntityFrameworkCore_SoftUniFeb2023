namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;

    using Common;
    using P01_StudentSystem.Data.Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConfig.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(s => s.PhoneNumber)
                .IsUnicode(false)
                .HasMaxLength(10);

            });
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.Property(r => r.Url)
                .IsUnicode(false);

            });
        }
    }
}