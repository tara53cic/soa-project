using ToursService.DTOs;

namespace ToursService.Services.Interfaces
{
    public interface ITouristPositionService
    {
        TouristPositionDto Upsert(TouristPositionDto positionDto);
        TouristPositionDto GetByTouristId(long touristId);
    }
}
