namespace ToursService.Models
{
    public class TourReview
    {
        public long Id { get; set; }
        public int Grade { get; set; }
        public string Comment { get; set; }
        public long TouristId { get; set; }
        public long TourId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime ReviewDate { get; set; }
        public List<string> Images { get; set; }

        public TourReview() { }

        public TourReview(int grade, string comment, long touristId, long tourId, DateTime attendanceDate, List<string>? images)
        {
            if (grade < 1 || grade > 5) throw new ArgumentException("Grade must be between 1 and 5.");
            if (string.IsNullOrWhiteSpace(comment)) throw new ArgumentNullException("Comment cannot be empty.");

            Grade = grade;
            Comment = comment;
            TouristId = touristId;
            TourId = tourId;
            AttendanceDate = attendanceDate;
            ReviewDate = DateTime.UtcNow;
            Images = images ?? new List<string>();
        }
    }
}
