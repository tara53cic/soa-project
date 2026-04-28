using ToursService.DTOs;
using ToursService.Models;
using ToursService.Repositories.Interfaces;
using ToursService.Services.Interfaces;

namespace ToursService.Services
{
    public class TouristPositionService : ITouristPositionService
    {
        private readonly ITouristPositionRepository _repository;

        public TouristPositionService(ITouristPositionRepository repository)
        {
            _repository = repository;
        }

        public TouristPositionDto Upsert(TouristPositionDto dto)
        {
            var existing = _repository.GetByTouristId(dto.TouristId);

            if (existing == null)
            {
                var newPos = new TouristPosition(dto.TouristId, dto.Latitude, dto.Longitude);
                return MapToDto(_repository.Create(newPos));
            }

            existing.Latitude = dto.Latitude;
            existing.Longitude = dto.Longitude;
            existing.LastActivity = DateTime.UtcNow;

            return MapToDto(_repository.Update(existing));
        }

        public TouristPositionDto GetByTouristId(long touristId)
        {
            var pos = _repository.GetByTouristId(touristId);
            return pos != null ? MapToDto(pos) : null;
        }

        private TouristPositionDto MapToDto(TouristPosition p)
        {
            return new TouristPositionDto
            {
                Id = p.Id,
                TouristId = p.TouristId,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                LastActivity = p.LastActivity
            };
        }
    }
}
