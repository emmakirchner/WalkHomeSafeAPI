using NetTopologySuite.Geometries;

namespace WalkHomeSafeAPI.Models.DTOs;

public class ReportDto
{
    public int Id { get; init; }

    public string UserName { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }

    public IReadOnlyCollection<ReportRatingDto> RatingCategories { get; init; } = [];

    public int UpvoteCount { get; set; }

    public int DownvoteCount {  get; set; }
}
