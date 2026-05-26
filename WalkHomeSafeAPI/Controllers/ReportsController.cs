using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkHomeSafeAPI.Extensions;
using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;
using WalkHomeSafeAPI.Services;

namespace WalkHomeSafeAPI.Controllers;

[ApiController]
[Route("api/reports")]
[Produces("application/json")]
public class ReportsController(IReportService reportService) : Controller
{
    /// <summary>
    /// Gets all reports.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<ReportDto>))]
    public ActionResult<IReadOnlyCollection<ReportDto>> GetAll([FromQuery] LocationDto location)
    {
        var result = reportService.GetAll(location);
        return Ok(result);
    }

    /// <summary>
    /// Creates or updates a report.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public ActionResult CreateOrUpdate(int? id, [FromBody] SaveReportDto saveReport)
    {
        var user = User.GetUserInfo();
        if (user == null)
        {
            return Unauthorized();
        }

        var result = id is null 
            ? reportService.Create(user, saveReport) 
            : reportService.Update(user, id.Value, saveReport);
        return result ? Ok() : BadRequest();
    }

    /// <summary>
    /// Deletes a report.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public ActionResult Delete(int id)
    {
        var user = User.GetUserInfo();
        if (user == null)
        {
            return Unauthorized();
        }

        var result = reportService.Delete(user, id);
        return result ? Ok() : BadRequest();
    }
}
