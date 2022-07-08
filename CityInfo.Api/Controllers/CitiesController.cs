using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CityInfo.Api.Controllers;

[ApiController]
[Route("api/cities")]
// [Route("api/{controller}")] -- this would use the classname without the controller part -- cities
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper; // automapper
    private const int maxCitiesPageSize = 20;

    // Inject repository contract
    public CitiesController(ICityInfoRepository  cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository;
        _mapper = mapper;
    }
    // [HttpGet("api/cities")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities([FromQuery(Name = "name")] string? name, 
        [FromQuery] string? searchQuery, 
        int pageNumber = 1, int pageSize = 10)
    {
        // can't go over the max page size here. 
        if (pageSize > maxCitiesPageSize)
        {
            pageSize = maxCitiesPageSize;
        }
        // returns a tuple so unpack it. 
        var (cityEntities, paginationMetadata) = await _cityInfoRepository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
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
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
    {
        // var cityToReturn = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);
        //
        // if (cityToReturn == null) return NotFound();
        // return Ok(cityToReturn);
            // return new JsonResult(_citiesDataStore.Cities.FirstOrDefault(c => c.Id == id));

            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            
            if (city == null) return NotFound();

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
    }
}