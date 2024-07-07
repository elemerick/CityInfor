using CityInfor.API.Entities;
using CityInfor.Models;

namespace CityInfor.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<City?> GetCityAsync(int cityId, bool ? includePointsOfInterest = false);
        Task<IEnumerable<PointOfInterest>> GetCityPointOfInterestAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestAsync(int cityId, int pointId);
        Task<PointOfInterest?> FindPointOfInterestAsync(int pointId);

        Task<bool> CityExistAsync(int cityId);

        Task CreatePointOfInterestAsync(int cityId, PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
    }
}
