namespace WalkHomeSafeAPI.Models.DTOs.Save;

public class SaveReportDto
{
    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public IReadOnlyCollection<ReportRatingDto> RatingCategories { get; init; } = [];
}
