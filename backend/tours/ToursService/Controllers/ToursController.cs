using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ToursService.DTOs;
using ToursService.Services.Interfaces;

namespace ToursService.Controllers
{
    [ApiController]
    [Route("api/tours")]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly IWebHostEnvironment _env;
        public ToursController(ITourService tourService, IWebHostEnvironment env)
        {
            _tourService = tourService;
            _env = env;
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
                [Consumes("multipart/form-data")]
                public async Task<ActionResult<TourDto>> AddKeyPoint(long id, [FromForm] KeyPointDto keyPointDto)
                {
                    try
                    {
                        // Log raw form values for debugging
                        var rawLon = Request.Form["Longitude"].ToString();
                        var rawLat = Request.Form["Latitude"].ToString();
                        Console.WriteLine($"Raw Longitude: '{rawLon}', Raw Latitude: '{rawLat}'");

                        // Parse explicitly using invariant culture to accept dot decimals reliably
                        if (!string.IsNullOrWhiteSpace(rawLon))
                            keyPointDto.Longitude = double.Parse(rawLon, CultureInfo.InvariantCulture);
                        if (!string.IsNullOrWhiteSpace(rawLat))
                            keyPointDto.Latitude = double.Parse(rawLat, CultureInfo.InvariantCulture);

                        if (keyPointDto.Image == null || keyPointDto.Image.Length == 0)
                        {
                            return BadRequest("Bad image.");
                        }

                        string wwwRootPath = _env.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(keyPointDto.Image.FileName);

                string imagesPath = Path.Combine(wwwRootPath, "images");
                if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);

                string fullPath = Path.Combine(imagesPath, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await keyPointDto.Image.CopyToAsync(fileStream);
                }

                keyPointDto.ImagePath = "images/" + fileName;

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

        [HttpGet("{id}")]
        public ActionResult<TourDto> Get(long id)
        {
            try
            {
                var result = _tourService.GetById(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/price")]
        public ActionResult<TourDto> UpdatePrice(long id, [FromBody] float price)
        {
            try
            {
                var result = _tourService.UpdatePrice(id, price);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/archive")]
        public ActionResult<TourDto> Archive(long id)
        {
            try
            {
                var result = _tourService.Archive(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPatch("{id}/unarchive")]
        public ActionResult<TourDto> Unarchive(long id)
        {
            try
            {
                var result = _tourService.Unarchive(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("keypoint/{keyPointId}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<TourDto>> UpdateKeyPoint(long keyPointId, [FromForm] KeyPointDto keyPointDto)
        {
            try
            {
                if (keyPointDto.Image != null && keyPointDto.Image.Length > 0)
                {
                    string wwwRootPath = _env.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(keyPointDto.Image.FileName);

                    string imagesPath = Path.Combine(wwwRootPath, "images");
                    if (!Directory.Exists(imagesPath)) Directory.CreateDirectory(imagesPath);

                    string fullPath = Path.Combine(imagesPath, fileName);

                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await keyPointDto.Image.CopyToAsync(fileStream);
                    }

                    keyPointDto.ImagePath = "images/" + fileName;
                }

                var result = _tourService.UpdateKeyPoint(keyPointId, keyPointDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<List<TourDto>> GetAll()
        {
            var result = _tourService.GetAll();
            return Ok(result);
        }

        [HttpDelete("{keyPointId}")]
        public IActionResult DeleteKeyPoint(long keyPointId)
        {
            _tourService.DeleteKeyPoint(keyPointId);
            return Ok();
        }
    }
}
