using System.ComponentModel.DataAnnotations;

namespace WalkHomeSafeAPI.Models.Entities;

public class UserEntity
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;
}
