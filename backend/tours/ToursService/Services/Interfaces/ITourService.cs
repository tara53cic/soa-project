using ToursService.DTOs;

namespace ToursService.Services.Interfaces
{
    public interface ITourService
    {
        TourDto Create(TourDto dto);
        TourDto AddKeyPoint(long tourId, KeyPointDto keyPointDto);
        TourDto Publish(long tourId, long authorId);
        List<TourDto> GetByAuthorId(long authorId);
        TourDto AddDuration(long tourId, TourDurationDto durationDto);
        TourDto GetById(long tourId);
        TourDto UpdatePrice(long tourId, float price);
        TourDto Archive(long tourId);
        TourDto Unarchive(long tourId);
    }
}
