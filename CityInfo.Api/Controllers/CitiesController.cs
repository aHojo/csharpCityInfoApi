using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/cities")]
// [Route("api/{controller}")] -- this would use the classname without the controller part -- cities
public class CitiesController : ControllerBase
{
    // [HttpGet("api/cities")]
    [HttpGet]
    public IActionResult GetCities()
    {
        return Ok(CitiesDataStore.Current.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var CityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

        if (CityToReturn == null) return NotFound();
        return Ok(CityToReturn);
            // return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
    }
}