using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public ActionResult CreateOrUpdate(int? id, [FromBody] SaveReportDto saveReport)
    {
        var result = id is null ? reportService.Create(saveReport) : reportService.Update(id.Value, saveReport);
        return result ? Ok() : BadRequest();
    }

    /// <summary>
    /// Deletes a report.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Delete(int id)
    {
        var result = reportService.Delete(id);
        return result ? Ok() : BadRequest();
    }
}
