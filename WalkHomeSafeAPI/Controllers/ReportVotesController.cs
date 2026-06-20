using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkHomeSafeAPI.Extensions;
using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;
using WalkHomeSafeAPI.Services;

namespace WalkHomeSafeAPI.Controllers;

[ApiController]
[Route("api/report-votes")]
[Produces("application/json")]

public class ReportVotesController(IReportService reportService) : Controller
{
    /// <summary>
    /// Gets all votes for reports of the current user.
    /// </summary>
    [HttpGet("by-user")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<ReportVoteDto>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public ActionResult<IReadOnlyCollection<ReportVoteDto>> GetVotesByUser()
    {
        var user = User.GetUserInfo();
        if (user == null)
        {
            return Unauthorized();
        }

        var result = reportService.GetVotesByUser(user);
        return Ok(result);
    }

    /// <summary>
    /// Creates or updates a user's votes for reports.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public ActionResult CreateOrUpdate([FromBody] IReadOnlyCollection<SaveReportVoteDto> votes)
    {
        var user = User.GetUserInfo();
        if (user == null)
        {
            return Unauthorized();
        }

        var result = reportService.CreateOrUpdateVotes(user, votes);
        return result ? Ok() : NotFound();
    }
}
