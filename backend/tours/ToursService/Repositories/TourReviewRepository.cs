using ToursService.Data;
using ToursService.Models;
using ToursService.Repositories.Interfaces;

namespace ToursService.Repositories
{
    public class TourReviewRepository : ITourReviewRepository
    {
        private readonly ToursDbContext _context;

        public TourReviewRepository(ToursDbContext context)
        {
            _context = context;
        }

        public TourReview CreateReview(TourReview review)
        {
            _context.TourReviews.Add(review);
            _context.SaveChanges();
            return review;
        }

        public List<TourReview> getByTourId(long tourId)
        {
            return _context.TourReviews.Where(r => r.TourId == tourId).ToList();
        }
    }
}
