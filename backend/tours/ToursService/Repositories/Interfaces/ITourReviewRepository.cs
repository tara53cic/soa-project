using ToursService.Models;

namespace ToursService.Repositories.Interfaces
{
    public interface ITourReviewRepository
    {
        TourReview CreateReview(TourReview review);
        List<TourReview> getByTourId(long tourId);
    }
}
