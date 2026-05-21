namespace WalkHomeSafeAPI.Models.Entities;

public class ReportVoteEntity
{
    public int Id { get; set; }

    public int ReportId { get; set; }
    public ReportEntity? Report { get; set; }

    public int UserId { get; set; }

    public bool IsUpvote { get; set; }
}
