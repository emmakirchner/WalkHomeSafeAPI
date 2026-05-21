using System.ComponentModel.DataAnnotations;

namespace WalkHomeSafeAPI.Models.DTOs;

public class ReportCategoryDto
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;
}
