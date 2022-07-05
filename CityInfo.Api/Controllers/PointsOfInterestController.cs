using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        // private readonly CitiesDataStore _citiesDataStore;

        // public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService localMailService, CitiesDataStore citiesDataStore )
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService localMailService, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _logger = logger ?? throw  new ArgumentNullException(nameof(logger));
            _localMailService = localMailService ?? throw  new ArgumentNullException(nameof(localMailService));
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
            // _citiesDataStore = citiesDataStore ?? throw  new ArgumentNullException(nameof(citiesDataStore));
            // HttpContext.RequestServices.GetService() can also use this
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            // try
            // {
            //     // throw new Exception("Exception example");
            //     // var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //
            //     if (city == null) 
            //     {
            //         _logger.LogInformation($"The city with ID {cityId} was not found when accessing this point of interest");
            //         return NotFound();
            //     }
            //     return Ok(city.PointsOfInterest);
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogCritical($"Exception whil getting points of interest for city with id {cityId}.", ex);
            //     return StatusCode(500, "A problem happened while handling your request");
            // }
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation("city with id {CityId} wasn\'t found when accessing points of interest.", cityId);
                return NotFound();
            }
            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }

        [HttpGet("{pointofinterestid}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId, int pointOfInterestId)
        {
            // var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            // if (city == null) return NotFound();
            //
            // var pointofinterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            // if (pointofinterest == null) return NotFound();
            //
            // return Ok(pointofinterest);
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation("city with id {CityId} wasn\'t found when accessing points of interest.", cityId);
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }
    
    
    
    [HttpPost]
    public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId,
        [FromBody] PointOfInterestForCreationDto pointOfInterest) // Dont need [FromBody] since it's inferred
    {
        // ModelState is a dictionary that contains the state of the model
        // ModelBinding validation 
        // Also has error messages for each thing submitted
    
        // We don't need to do this, because in program.cs we set up apicontroller service 
        //if (!ModelState.IsValid) // this is false if one of the values has an error
        //{
        //    return BadRequest();
        //}
        // var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        // if (city == null) return NotFound();
        //
        // // for the demo, will be improved later
        // // select many here gets all of the points of interest we have
        // var maxPointOfInterestId =
        //     _citiesDataStore.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
        // var finalPointOfInterest = new PointOfInterestDto()
        // {
        //     Id = ++maxPointOfInterestId,
        //     Name = pointOfInterest.Name,
        //     Description = pointOfInterest.Description
        // };
        // city.PointsOfInterest.Add(finalPointOfInterest);
        // // CreatedAtRoute returns a location header to get the point of interest. 
        // // for example https://localhost:7182/api/cities/3/pointsofinterest/7
        // return CreatedAtRoute("GetPointOfInterest", new { cityId, pointofinterestid = finalPointOfInterest.Id },
        //     finalPointOfInterest);
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

        await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
        await _cityInfoRepository.SaveChangesAsync();

        var createdPointOfInterest = _mapper.Map<Models.PointOfInterestDto>(finalPointOfInterest);


        return CreatedAtRoute("GetPointOfInterest", new
        {
            cityId = cityId,
            pointOfInterestId = createdPointOfInterest.Id
        }, createdPointOfInterest);
    }
    
    [HttpPut("{pointOfInterestId}")]
    public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
        PointOfInterestForUpdateDto pointOfInterest)
    {
        // var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        // if (city == null) return NotFound();
        //
        // var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        // if (pointOfInterestFromStore == null)
        // {
        //     return NotFound();
        // }
        //
        // pointOfInterestFromStore.Name = pointOfInterest.Name;
        // pointOfInterestFromStore.Description = pointOfInterest.Description;
        //

        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }
        
        // Automapper here overrides values in the destination from those in the source, eg pointOfInterest -> pointOfInterestEntity
        _mapper.Map(pointOfInterest, pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpPatch("{pointofinterestid}")]
    public async Task<ActionResult> PartiallyUpdatedPointOfInterest(int cityId, int pointOfInterestId,
        JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
    {
        // var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
        // if (city == null)
        // {
        //     return NotFound();
        // }
        //
        // var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
        // if (pointOfInterestFromStore == null)
        // {
        //     return NotFound();
        // }
        //
        // var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
        // {
        //     Name = pointOfInterestFromStore.Name,
        //     Description = pointOfInterestFromStore.Description
        // };
        // patchDocument.ApplyTo(pointOfInterestToPatch,
        //     ModelState); // any errors in modelstate will send a bad request
        // if (!ModelState.IsValid)
        // {
        //     return BadRequest(ModelState);
        // }
        //
        // if (!TryValidateModel(pointOfInterestToPatch))
        // {
        //     return BadRequest(ModelState);
        // }
        //
        // pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
        // pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;
        
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }

        var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
        patchDocument.ApplyTo(pointOfInterestToPatch,
            ModelState); // any errors in modelstate will send a bad request
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (!TryValidateModel(pointOfInterestToPatch))
        {
            return BadRequest(ModelState);
        }

        // map back to entity so that we can save the changes to the db. 
        _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpDelete("{pointOfInterestId}")]
    public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
    {
        if (!await _cityInfoRepository.CityExistsAsync(cityId))
        {
            return NotFound();
        }

        var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
        if (pointOfInterestEntity == null)
        {
            return NotFound();
        }
        
    
        _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
        await _cityInfoRepository.SaveChangesAsync();
        _localMailService.Send("Point of interest Deleted", $"Point of interest was deleted >> {pointOfInterestEntity.Name} with ID of {pointOfInterestEntity.Id}");
        
        return NoContent();
    }
    }
}
