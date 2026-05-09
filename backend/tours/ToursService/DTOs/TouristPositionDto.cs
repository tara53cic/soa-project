namespace ToursService.DTOs
{
    public class TouristPositionDto
    {
        public long Id { get; set; }
        public long TouristId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
