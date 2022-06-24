using System.ComponentModel.DataAnnotations;

namespace CityInfo.Api.Models;
// This DTO is for creating updating and deleting resouse
public class PointOfInterestForCreationDto
{
    
    [Required(ErrorMessage = "You should give a value for the Name")]
    [MaxLength(50)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
}