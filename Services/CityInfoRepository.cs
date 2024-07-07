using CityInfor.API.Entities;
using CityInfor.DbContexts;
using CityInfor.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfor.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _cityContext;
        
        public CityInfoRepository(CityInfoContext cityInfoContext)
        {
            _cityContext = cityInfoContext;
        }

        public async Task<bool> CityExistAsync(int cityId)
        {
            return await _cityContext.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _cityContext.SaveChangesAsync() >= 1;
        }


        public async Task CreatePointOfInterestAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await _cityContext.Cities.FindAsync(cityId);
            if (city != null) {
                city.PointOfInterests.Add(pointOfInterest);
            }
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {   
            if (pointOfInterest != null) {
                _cityContext.PointOfInterests.Remove(pointOfInterest);
            }
        }

        public async Task<PointOfInterest?> FindPointOfInterestAsync(int pointId)
        {
            return await _cityContext.PointOfInterests.FindAsync(pointId);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            var cities = await _cityContext.Cities.OrderBy(c => c.Name).ToListAsync();
            return cities;
        }

        public async Task<City?> GetCityAsync(int cityId, bool? includePointsOfInterest = false)
        {
            var city = includePointsOfInterest == true
                ? await _cityContext.Cities.Include(c => c.PointOfInterests)
                    .Where(city => city.Id == cityId).FirstOrDefaultAsync()
                : await _cityContext.Cities.Where(city => city.Id == cityId).FirstOrDefaultAsync(); ;
            return city;
        }

        public async Task<IEnumerable<PointOfInterest>> GetCityPointOfInterestAsync(int cityId)
        {
            var pointsOfInterest = await _cityContext.PointOfInterests
                .Where(poi => poi.CityId == cityId).ToListAsync();
            return pointsOfInterest;
        }

        public async Task<PointOfInterest ?> GetPointOfInterestAsync(int cityId, int pointId)
        {
            var pointOfInterest = await _cityContext.PointOfInterests
                .Where(poi => poi.CityId == cityId && poi.Id == pointId)
                .FirstOrDefaultAsync();
            return pointOfInterest;
        }

        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name, 
            string? searchQuery, 
            int pageNumber, 
            int pageSize)
        {
            /*if(string.IsNullOrEmpty(name) && string.IsNullOrWhiteSpace(searchQuery))
                return await GetCitiesAsync();*/

            var collection = _cityContext.Cities as IQueryable<City>;
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => c.Name.Contains(searchQuery)
                || c.Description != null && c.Description.Contains(searchQuery));
            }
            int totalItemCount = await collection.CountAsync();
            var metadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var dataCollection = await collection.OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (dataCollection,  metadata);
        }
    }
}
