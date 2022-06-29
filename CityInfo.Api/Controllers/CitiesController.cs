using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/cities")]
// [Route("api/{controller}")] -- this would use the classname without the controller part -- cities
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper; // automapper

    // Inject repository contract
    public CitiesController(ICityInfoRepository  cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository;
        _mapper = mapper;
    }
    // [HttpGet("api/cities")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
    {
        var cityEntities = await _cityInfoRepository.GetCitiesAsync();
        /*
         * THIS IS REPLACED WITH AUTOMAPPER
         
        var results = new List<CityWithoutPointsOfInterestDto>();
        foreach (var cityEntity in cityEntities)
        {
            results.Add(new CityWithoutPointsOfInterestDto
            {
                Id = cityEntity.Id,
                Description = cityEntity.Description,
                Name = cityEntity.Name
            });
        }
        return Ok(results);
        */
        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    // [HttpGet("{id}")]
    // public ActionResult<CityDto> GetCity(int id)
    // {
    //     // var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
    //     //
    //     // if (cityToReturn == null) return NotFound();
    //     // return Ok(cityToReturn);
    //         // return new JsonResult(_citiesDataStore.Cities.FirstOrDefault(c => c.Id == id));
    // }
}