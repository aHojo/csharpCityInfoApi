using System.ComponentModel.DataAnnotations;

namespace CityInfo.Api.Models;

public class PointOfInterestForUpdateDto
{
    [Required(ErrorMessage = "You should give a value for the Name")]
    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
}