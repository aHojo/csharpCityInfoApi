﻿namespace CityInfo.Api.Models;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public int NumberOfPointsOfInterest { get; set; }
    public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();


}