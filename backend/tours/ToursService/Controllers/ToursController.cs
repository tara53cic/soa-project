using Microsoft.AspNetCore.Mvc;
using ToursService.DTOs;
using ToursService.Services.Interfaces;

namespace ToursService.Controllers
{
    [ApiController]
    [Route("api/tours")]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;
        public ToursController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpPost]
        public ActionResult Create([FromBody] TourDto tourDto)
        {
            try
            {
                var result = _tourService.Create(tourDto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/keypoint")]
        public ActionResult<TourDto> AddKeyPoint(long id, [FromBody] KeyPointDto keyPointDto)
        {
            try
            {
                var result = _tourService.AddKeyPoint(id, keyPointDto);
                return Ok(result);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/publish")]
        public ActionResult<TourDto> Publish(long id, [FromBody] long authorId)
        {
            try
            {
                var result = _tourService.Publish(id, authorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("author/{authorId}")]
        public ActionResult<List<TourDto>> GetByAuthor(long authorId)
        {
            var result = _tourService.GetByAuthorId(authorId);
            return Ok(result);
        }

        [HttpPost("{id}/duration")]
        public ActionResult<TourDto> AddDuration(long id, [FromBody] TourDurationDto durationDto)
        {
            try
            {
                var result = _tourService.AddDuration(id, durationDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
