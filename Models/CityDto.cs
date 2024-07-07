using System.Collections.ObjectModel;

namespace CityInfor.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public int NumberOfPointOfInterest { get => PointOfInterests.Count; }

        public Collection<PointOfInterestDto> PointOfInterests { get; set; } = new Collection<PointOfInterestDto>();
    }

    public class CityWithoutPointOfInterestsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
