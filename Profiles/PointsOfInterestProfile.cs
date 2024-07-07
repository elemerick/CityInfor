using AutoMapper;

namespace CityInfor.Profiles
{
    public class PointsOfInterestProfile : Profile
    {
        public PointsOfInterestProfile()
        {
            CreateMap<API.Entities.PointOfInterest, API.Models.PointOfInterestDto>();
            CreateMap<API.Entities.PointOfInterest, API.Models.PointOfInterestForUpdateDto>();
            CreateMap<API.Models.PointOfInterestForCreationDto, API.Entities.PointOfInterest>();
        }
    }
}
