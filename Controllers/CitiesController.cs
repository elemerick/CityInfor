using AutoMapper;
using CityInfor.API.Models;
using CityInfor.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfor.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repo;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestsDto>>> GetCities(
            string ? name, 
            string ? searchQuery,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageSize > maxCitiesPageSize) pageSize = maxCitiesPageSize;
            var (cities, paginationMetadata) = await _repo.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
            var records = _mapper.Map<IEnumerable<CityWithoutPointOfInterestsDto>>(cities);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false) {
            var city = await _repo.GetCityAsync(id, includePointsOfInterest);
            if (city == null) return NotFound();
            
            return includePointsOfInterest == true 
                ? Ok(_mapper.Map<CityDto>(city))
                : Ok(_mapper.Map<CityWithoutPointOfInterestsDto>(city));
        }
    }
}
