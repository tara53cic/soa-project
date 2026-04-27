using ToursService.Models;

namespace ToursService.Repositories.Interfaces
{
    public interface ITouristPositionRepository
    {
        TouristPosition GetByTouristId(long touristId);
        TouristPosition Create(TouristPosition position);
        TouristPosition Update(TouristPosition position);
    }
}
