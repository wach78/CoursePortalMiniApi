using CoursePortalMiniApi.Enums;
namespace CoursePortalMiniApi.Models
{
    public sealed class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public int DurationInWeeks { get; set; }
        public int Price { get; set; }
        public CourseLevel Level { get; set; }
    }
}
