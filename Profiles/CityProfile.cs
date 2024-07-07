using AutoMapper;


namespace CityInfor.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<API.Entities.City, API.Models.CityWithoutPointOfInterestsDto>();
            CreateMap<API.Entities.City, API.Models.CityDto>();
        }
    }
}
