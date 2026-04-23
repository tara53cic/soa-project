namespace ToursService.Models
{
    public class TourDuration
    {
        public long Id { get; set; }
        public TravelType TravelType { get; set; }
        public double Minutes { get; set; }

        public TourDuration() { }
        public TourDuration(double minutes, TravelType travelType)
        {
            TravelType = travelType;
            Minutes = minutes;
        }
    }
}
