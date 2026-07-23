using System.Reflection.Emit;
using CoursePortalMiniApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursePortalMiniApi.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses => Set<Course>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");

                entity.HasKey(course => course.Id);

                entity.Property(course => course.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(course => course.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(course => course.StartDate)
                    .IsRequired();

                entity.Property(course => course.DurationInWeeks)
                    .IsRequired();

                entity.Property(course => course.Price)
                    .IsRequired();

                entity.Property(course => course.Level)
                    .IsRequired();
            });
        }
    }
}
