using ToursService.DTOs;

namespace ToursService.Services.Interfaces
{
    public interface ITourReviewService
    {
        TourReviewDto CreateReview(TourReviewDto reviewDto);
        List<TourReviewDto> GetByTourId(long tourId);
    }
}
