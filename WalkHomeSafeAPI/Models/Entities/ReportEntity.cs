using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace WalkHomeSafeAPI.Models.Entities;

public class ReportEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserEntity? User { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Point? Location { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<ReportRatingEntity> Ratings { get; set; } = [];
    public ICollection<ReportVoteEntity> Votes { get; set; } = [];
}
