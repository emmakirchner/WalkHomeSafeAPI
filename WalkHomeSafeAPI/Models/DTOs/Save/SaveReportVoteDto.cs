namespace WalkHomeSafeAPI.Models.DTOs.Save;

public class SaveReportVoteDto
{
    public int ReportId { get; init; }

    public bool? IsUpvote { get; init; }
}
