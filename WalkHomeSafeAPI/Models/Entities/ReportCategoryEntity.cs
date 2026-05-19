using System.ComponentModel.DataAnnotations;

namespace WalkHomeSafeAPI.Models.Entities;

public class ReportCategoryEntity
{
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
