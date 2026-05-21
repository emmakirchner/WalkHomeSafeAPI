namespace WalkHomeSafeAPI.Models.DTOs;

public class LocationDto
{
    public double? Latitude { get; init; }

    public double? Longitude { get; init; }

    public int? RadiusInMeters { get; init; }
}
