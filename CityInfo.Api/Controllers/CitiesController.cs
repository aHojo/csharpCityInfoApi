using CityInfo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/cities")]
// [Route("api/{controller}")] -- this would use the classname without the controller part -- cities
public class CitiesController : ControllerBase
{
    private readonly CitiesDataStore _citiesDataStore;

    public CitiesController(CitiesDataStore citiesDataStore )
    {
        _citiesDataStore = citiesDataStore;
    }
    // [HttpGet("api/cities")]
    [HttpGet]
    public IActionResult GetCities()
    {
        return Ok(_citiesDataStore.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

        if (cityToReturn == null) return NotFound();
        return Ok(cityToReturn);
            // return new JsonResult(_citiesDataStore.Cities.FirstOrDefault(c => c.Id == id));
    }
}