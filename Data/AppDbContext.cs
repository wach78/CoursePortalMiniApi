using CoursePortalMiniApi.Constants;
using CoursePortalMiniApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursePortalMiniApi.Data;

public sealed class AppDbContext(
    DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
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
                .HasMaxLength(CourseValidationConstants.NameMaxLength);

            entity.Property(course => course.Description)
                .IsRequired()
                .HasMaxLength(CourseValidationConstants.DescriptionMaxLength);

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
