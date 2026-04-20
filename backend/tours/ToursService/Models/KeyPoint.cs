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

    }
}
