using AutoMapper;
using CityInfor.API.Entities;
using CityInfor.API.Models;
using CityInfor.API.Services;
using CityInfor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfor.API.Controllers
{
    [Route("api/cities/{cityId}/[controller]")]
    [ApiController]
    public class PointOfInterestsController : ControllerBase
    {
        private readonly ILogger<PointOfInterestsController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _repo;
        private readonly IMapper _mapper;

        public PointOfInterestsController(ILogger<PointOfInterestsController> logger, 
            IMailService mailService,
            IMapper mapper,
            ICityInfoRepository cityInfoRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService;
            _repo = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetCityPointOfInterest(int cityId)
        {
            //throw new Exception("AN error occured");
            if (!await _repo.CityExistAsync(cityId)) return NotFound();
            var pointsOfInterests = await _repo.GetCityPointOfInterestAsync(cityId);
            
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterests));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId) {
            try
            {
                //throw new Exception("AN error occured");
                
                if (!await _repo.CityExistAsync(cityId))
                {
                    _logger.LogInformation($"The city with the ID {cityId} was not found");
                    return NotFound();
                }

                var pointOfInterest = await _repo.GetPointOfInterestAsync(cityId, pointOfInterestId);
                if (pointOfInterest == null) return NotFound();
                return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
            }
            catch (Exception ex) {
                _logger.LogCritical($"An Error occured while getting the points if interest of City {cityId}", ex);
                return StatusCode(500, $"An Error occured while getting the points if interest of City {cityId}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, [FromBody]PointOfInterestForCreationDto pointOfInterest)
        {   
            if(! await _repo.CityExistAsync(cityId)) return NotFound();


            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);
            await _repo.CreatePointOfInterestAsync(cityId, finalPointOfInterest);
            await _repo.SaveChangesAsync();
            var createdPointOfInterest = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new { cityId, pointOfInterestId = createdPointOfInterest.Id }, createdPointOfInterest);
        }

        [HttpPut("pointOfInterestId")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, [FromBody] PointOfInterestForUpdateDto pointOfInterest) {
            
            if (!await _repo.CityExistAsync(cityId)) 
                return NotFound();

            var pointOfInterestForUpdate = await _repo.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestForUpdate == null) 
                return NotFound();
            
            _mapper.Map(pointOfInterest, pointOfInterestForUpdate);
            await _repo.SaveChangesAsync();
            
            return NoContent();
        }


        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _repo.CityExistAsync(cityId))
                return NotFound();

            var pointOfInterestForUpdate = await _repo.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestForUpdate == null)
                return NotFound();
             
            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestForUpdate);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if(!TryValidateModel(pointOfInterestToPatch)) return BadRequest(ModelState);

            _mapper.Map(pointOfInterestToPatch, pointOfInterestForUpdate);
            await _repo.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _repo.CityExistAsync(cityId))
                return NotFound();

            var pointOfInterestEntity = await _repo.GetPointOfInterestAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
                return NotFound();
            _mailService.Send("Point of interest deleted", $"Be aware that the point of interest with the ID: " +
                $"{pointOfInterestId} and Name: {pointOfInterestEntity.Name} is deleted");

            _repo.DeletePointOfInterest(pointOfInterestEntity);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
