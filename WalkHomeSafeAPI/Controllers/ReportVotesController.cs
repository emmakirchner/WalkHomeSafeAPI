using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkHomeSafeAPI.Models.DTOs.Save;
using WalkHomeSafeAPI.Services;

namespace WalkHomeSafeAPI.Controllers;

[ApiController]
[Route("api/report-votes")]
[Produces("application/json")]

public class ReportVotesController(IReportService reportService) : Controller
{
    /// <summary>
    /// Creates or updates a user's votes for reports.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult CreateOrUpdate(string username, [FromBody] IReadOnlyCollection<SaveReportVoteDto> votes)
    {
        var result = reportService.CreateOrUpdateVotes(username, votes);
        return result ? Ok() : NotFound();
    }
}
