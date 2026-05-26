using System.ComponentModel.DataAnnotations;

namespace WalkHomeSafeAPI.Models.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string FirebaseId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
