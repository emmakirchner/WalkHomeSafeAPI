using System.ComponentModel.DataAnnotations;

namespace WalkHomeSafeAPI.Models.Entities;

public class ReportRatingEntity
{
    public int Id { get; set; }

    public int ReportId { get; set; }
    public ReportEntity? Report { get; set; }

    public int CategoryId { get; set; }
    public ReportCategoryEntity? Category { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }
}
