﻿using CityInfo.Api.Entities;

namespace CityInfo.Api.Services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
    // If we use this, the consumer can keep building on it
    // EG. add Orderby Where before the query executed. 
    // IQueryable<City> GetCities();
    // Can be null cause we might not get a city back
    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
    Task<bool> CityExistsAsync(int cityId);

    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

    Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
    Task<bool> SaveChangesAsync();

    void DeletePointOfInterest(PointOfInterest pointOfInterest);

}