namespace ToursService.DTOs
{
    public class KeyPointDto
    {
        public long Id { get; set; }
        public long TourId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string? ImagePath { get; set; }
        public IFormFile? Image { get; set; }
    }
}
