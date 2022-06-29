using AutoMapper;

namespace CityInfo.Api.Profiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        // Maps property names on the source object to properties on the destination
        // Ignores NullReferenceExceptions types (If the prop doesn't exist, it will be ignored)
        CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
    }
}