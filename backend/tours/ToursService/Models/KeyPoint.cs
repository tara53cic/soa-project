namespace ToursService.Models
{
    public class KeyPoint
    {
        public long Id { get; set; }
        public long TourId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public double Longitude { get; init; }
        public double Latitude { get; init; }
        public string ImagePath { get; init; }

        public KeyPoint() { }

        public KeyPoint(long tourId, string name, string description, double longitude, double latitude, string imagePath)
        {
            if (tourId <= 0) throw new ArgumentException("Invalid TourId.");
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Invalid name.");
            if (longitude < -180 || longitude > 180) throw new ArgumentException("Invalid Longitude");
            if (latitude < -90 || latitude > 90) throw new ArgumentException("Invalid Latitude");
            if (string.IsNullOrWhiteSpace(imagePath)) throw new ArgumentException("Invalid image path.");

            TourId = tourId;
            Name = name;
            Description = description;
            Longitude = longitude;
            Latitude = latitude;
            ImagePath = imagePath;
        }
    }
}
