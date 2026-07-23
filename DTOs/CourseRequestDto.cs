using System.ComponentModel.DataAnnotations;
using CoursePortalMiniApi.Constants;
using CoursePortalMiniApi.Enums;

namespace CoursePortalMiniApi.DTOs;

public sealed class CourseRequestDto
{
    [Required]
    [StringLength(CourseValidationConstants.NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(CourseValidationConstants.DescriptionMaxLength)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateOnly StartDate { get; set; }

    [Range(
        CourseValidationConstants.DurationMinWeeks,
        CourseValidationConstants.DurationMaxWeeks)]
    public int DurationInWeeks { get; set; }

    [Range(
        typeof(decimal),
        CourseValidationConstants.PriceMinValue,
        CourseValidationConstants.PriceMaxValue)]
    public decimal Price { get; set; }

    public CourseLevel Level { get; set; }
}
