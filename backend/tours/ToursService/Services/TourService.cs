using ToursService.DTOs;
using ToursService.Models;
using ToursService.Repositories.Interfaces;
using ToursService.Services.Interfaces;

namespace ToursService.Services
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _repository;

        public TourService(ITourRepository repository)
        {
            _repository = repository; 
        }

        public TourDto Create(TourDto dto)
        {
            var tour = new Tour(dto.Name, dto.Description, (TourDifficulty)dto.Difficulty, dto.AuthorId, dto.Tags);

            var savedTour = _repository.Create(tour);
            return MapToDto(savedTour);
        }

        public TourDto AddKeyPoint(long tourId, KeyPointDto kpDto)
        {
            var tour = _repository.GetById(tourId);

            var keyPoint = new KeyPoint(tourId, kpDto.Name, kpDto.Description, kpDto.Longitude, kpDto.Latitude, kpDto.ImagePath);

            tour.AddKeyPoint(keyPoint);

            _repository.Update(tour);
            return MapToDto(tour);
        }

        public TourDto Publish(long tourId, long authorId)
        {
            var tour = _repository.GetById(tourId);

            tour.Publish(authorId);

            _repository.Update(tour);
            return MapToDto(tour);
        }

        private TourDto MapToDto(Tour tour)
        {
            return new TourDto
            {
                Id = tour.Id,
                Name = tour.Name,
                Description = tour.Description,
                DistanceInKm = tour.DistanceInKm,
                Status = (TourStatusDto)tour.Status,
                AuthorId = tour.AuthorId
            };
        }

        public List<TourDto> GetByAuthorId(long authorId)
        {
            return _repository.GetByAuthorId(authorId).Select(MapToDto).ToList();
        }

    }
}
