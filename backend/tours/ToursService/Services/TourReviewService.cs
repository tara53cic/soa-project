using ToursService.DTOs;
using ToursService.Models;
using ToursService.Repositories.Interfaces;
using ToursService.Services.Interfaces;

namespace ToursService.Services
{
    public class TourReviewService : ITourReviewService
    {
        private readonly ITourReviewRepository _tourReviewRepository;

        public TourReviewService(ITourReviewRepository reviewRepository)
        {
            _tourReviewRepository = reviewRepository;
        }

        public TourReviewDto CreateReview(TourReviewDto reviewDto)
        {
            var review = new TourReview(
                reviewDto.Grade,
                reviewDto.Comment,
                reviewDto.TouristId,
                reviewDto.TourId,
                reviewDto.TouristUsername,
                reviewDto.AttendanceDate,
                reviewDto.Images
            );

            var savedReview = _tourReviewRepository.CreateReview(review);
            return MapToDto(savedReview);
        }

        public List<TourReviewDto> GetByTourId(long tourId)
        {
            var reviews = _tourReviewRepository.GetByTourId(tourId);
            return reviews.Select(MapToDto).ToList();
        }

        private TourReviewDto MapToDto(TourReview review)
        {
            return new TourReviewDto
            {
                Id = review.Id,
                Grade = review.Grade,
                Comment = review.Comment,
                TouristId = review.TouristId,
                AttendanceDate = review.AttendanceDate,
                ReviewDate = review.ReviewDate,
                Images = review.Images,
                TourId = review.TourId,
                TouristUsername = review.TouristUsername
            };
        }
    }
}
