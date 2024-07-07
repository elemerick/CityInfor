using CityInfor.API.Models;
using System.Collections.ObjectModel;

namespace CityInfor.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public CitiesDataStore() { 
            this.Cities = new List<CityDto>()
            {
                new CityDto { 
                    Id = 1, 
                    Name = "Vilnius", 
                    Description = "The Capital city",
                    PointOfInterests = new Collection<PointOfInterestDto>()
                    {
                        new PointOfInterestDto { Id = 1, Name = "Gedimino Bokstas", Description = "Iconic point in vilnius"},
                        new PointOfInterestDto { Id = 2, Name = "Three Statues", Description = "Iconic point in vilnius"}
                    }
                },
                new CityDto { 
                    Id = 2, 
                    Name = "Kaunas", 
                    Description = "The second biggest city in Lithuania",
                    PointOfInterests = new Collection<PointOfInterestDto>()
                    {
                        new PointOfInterestDto { Id = 3, Name = "Kaunas Airport", Description = "Kaunas international airport"},
                        new PointOfInterestDto { Id = 4, Name = "Kaunas Central Station", Description = "Kaunas central rails stations"}
                    }
                },
                new CityDto {
                    Id = 3,
                    Name = "Klapedia",
                    Description = "at the shore of the baltic sea, it's the ideal place to enjoy beach during summer",
                    PointOfInterests = new Collection<PointOfInterestDto>()
                    {
                        new PointOfInterestDto { Id = 5, Name = "Klaipeda Beach", Description = "Klaipeda international airport"},
                        new PointOfInterestDto { Id = 6, Name = "Palanga Beach", Description = "Palanga central rails stations"}
                    }
                }
            };
        }
    }
}
