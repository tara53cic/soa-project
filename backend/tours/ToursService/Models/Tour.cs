using System.Security.Cryptography;

namespace ToursService.Models
{
    public class Tour
    {
        public long Id { get; set; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public TourDifficulty Difficulty { get; init; }
        public List<string>? Tags { get; init; }
        public float Price { get; set; } = 0;
        public TourStatus Status { get; set; } = TourStatus.DRAFT;
        public List<KeyPoint>? KeyPoints { get; set; }
        public long AuthorId { get; set; }
        public double DistanceInKm { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public DateTime? ArchivedDateTime { get; set; }
        public List<TourDuration> Durations { get; set; } = new List<TourDuration>();

    }
}
