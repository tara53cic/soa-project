using Microsoft.AspNetCore.Mvc;
using ToursService.DTOs;
using ToursService.Services.Interfaces;

namespace ToursService.Controllers
{
    [ApiController]
    [Route("api/tourist/position")]
    public class TouristPositionController : ControllerBase
    {
        private readonly ITouristPositionService _positionService;

        public TouristPositionController(ITouristPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpPost]
        public ActionResult<TouristPositionDto> Upsert([FromBody] TouristPositionDto positionDto)
        {
            try
            {
                var result = _positionService.Upsert(positionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{touristId}")]
        public ActionResult<TouristPositionDto> GetByTourist(long touristId)
        {
            var result = _positionService.GetByTouristId(touristId);
            if (result == null) return NotFound("Position not set for this tourist.");
            return Ok(result);
        }
    }
}
