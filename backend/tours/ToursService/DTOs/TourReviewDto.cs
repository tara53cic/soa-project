namespace ToursService.DTOs
{
    public class TourReviewDto
    {
        public long Id { get; set; }
        public int Grade { get; set; }
        public string Comment { get; set; }
        public long TouristId { get; set; }
        public long TourId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public List<string>? Images { get; set; }
    }
}
