using Microsoft.AspNetCore.Mvc;
using WalkHomeSafeAPI.Models.DTOs;
using WalkHomeSafeAPI.Models.DTOs.Save;
using WalkHomeSafeAPI.Services;

namespace WalkHomeSafeAPI.Controllers;

[ApiController]
[Route("api/report-categories")]
[Produces("application/json")]
public class ReportCategoriesController(ICategoryService categoryService) : Controller
{
    /// <summary>
    /// Gets all report categories.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<ReportCategoryDto>))]
    public ActionResult<IReadOnlyCollection<ReportDto>> GetAll()
    {
        var result = categoryService.GetAll();
        return Ok(result);
    }

    /// <summary>
    /// Creates or updates a report category.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult CreateOrUpdate(int? id, [FromBody] SaveReportCategoryDto saveReportCategory)
    {
        var result = id is null ? categoryService.Create(saveReportCategory) : categoryService.Update(id.Value, saveReportCategory);
        return result ? Ok() : BadRequest();
    }

    /// <summary>
    /// Deletes a report category.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult Delete(int id)
    {
        var result = categoryService.Delete(id);
        return result ? Ok() : BadRequest();
    }
}
