namespace ToursService.Models
{
    public class TouristPosition
    {
        public long Id { get; set; }
        public long TouristId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastActivity { get; set; }

        public TouristPosition() { }

        public TouristPosition(long touristId, double latitude, double longitude)
        {
            TouristId = touristId;
            Latitude = latitude;
            Longitude = longitude;
            LastActivity = DateTime.UtcNow;
        }
    }
}
