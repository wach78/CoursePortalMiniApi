namespace CoursePortalMiniApi.Constants;

public static class CourseValidationConstants
{
    public const int NameMaxLength = 100;
    public const int DescriptionMaxLength = 1000;

    public const int DurationMinWeeks = 1;
    public const int DurationMaxWeeks = 55;

    public const string PriceMinValue = "0";
    public const string PriceMaxValue = "99999.99";
}
