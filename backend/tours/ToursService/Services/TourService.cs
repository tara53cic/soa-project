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
                Difficulty = (TourDifficultyDto)tour.Difficulty,
                Tags = tour.Tags,
                Price = tour.Price,
                DistanceInKm = tour.DistanceInKm,
                Status = (TourStatusDto)tour.Status,
                AuthorId = tour.AuthorId,
                PublishedDateTime = tour.PublishedDateTime,
                ArchivedDateTime = tour.ArchivedDateTime,
                KeyPoints = tour.KeyPoints?.Select(kp => new KeyPointDto
                {
                    Id = kp.Id,
                    Name = kp.Name,
                    Description = kp.Description,
                    Latitude = kp.Latitude,
                    Longitude = kp.Longitude,
                    ImagePath = kp.ImagePath
                }).ToList(),

                Duration = tour.Durations?.Select(d => new TourDurationDto
                {
                    TravelType = (TravelTypeDto)d.TravelType,
                    Minutes = d.Minutes
                }).ToList()
            };
        }

        public List<TourDto> GetByAuthorId(long authorId)
        {
            return _repository.GetByAuthorId(authorId).Select(MapToDto).ToList();
        }

        public TourDto AddDuration(long tourId, TourDurationDto dto)
        {
            var tour = _repository.GetById(tourId);
            if (tour == null) throw new Exception("Tour not found");

            tour.AddDuration(dto.Minutes, (TravelType)dto.TravelType);

            _repository.Update(tour);

            return MapToDto(tour);
        }

        public TourDto GetById(long tourId)
        {
            var tour = _repository.GetById(tourId);
            if (tour == null) throw new Exception("Tour not found");
            return MapToDto(tour);
        }

        public TourDto UpdatePrice(long tourId, float price)
        {
            var tour = _repository.GetById(tourId);
            if (tour == null) throw new Exception("Tour not found");

            tour.Price = price;
            _repository.Update(tour);

            return MapToDto(tour);
        }

        public TourDto Archive(long tourId)
        {
            var tour = _repository.GetById(tourId);
            if (tour == null) throw new Exception("Tour not found");

            tour.Status = TourStatus.ARCHIVED;
            tour.ArchivedDateTime = DateTime.Now;
            _repository.Update(tour);

            return MapToDto(tour);
        }

        public TourDto Unarchive(long tourId)
        {
            var tour = _repository.GetById(tourId);
            if (tour == null) throw new Exception("Tour not found");

            tour.Status = TourStatus.CONFIRMED;
            _repository.Update(tour);

            return MapToDto(tour);
        }

        public TourDto UpdateKeyPoint(long tourId, KeyPointDto kpDto)
        {
            var tour = _repository.GetById(tourId);

            var kp = tour.KeyPoints.FirstOrDefault(x => x.Id == kpDto.Id);
            if (kp == null) throw new Exception("KeyPoint not found");

            kp.Name = kpDto.Name;
            kp.Description = kpDto.Description;
            kp.Latitude = kpDto.Latitude;
            kp.Longitude = kpDto.Longitude;

            if (!string.IsNullOrEmpty(kpDto.ImagePath))
            {
                kp.ImagePath = kpDto.ImagePath;
            }

            _repository.Update(tour);

            return MapToDto(tour);
        }

        public List<TourDto> GetAll()
        {
            var tours = _repository.GetAll();
            return tours.Select(MapToDto).ToList();
        }
    }
}
