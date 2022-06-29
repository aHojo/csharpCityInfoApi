using CityInfo.Api.Entities;

namespace CityInfo.Api.Services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();
    // If we use this, the consumer can keep building on it
    // EG. add Orderby Where before the query executed. 
    // IQueryable<City> GetCities();
    // Can be null cause we might not get a city back
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
    
}