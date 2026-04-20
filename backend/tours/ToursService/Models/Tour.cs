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

        public Tour() { }

        public Tour(string name, string description, TourDifficulty difficulty, long authorId, List<string> tags)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Invalid name.");
            if (authorId <= 0) throw new ArgumentOutOfRangeException("Invalid authorid.");

            Name = name;
            Description = description;
            Difficulty = difficulty;
            AuthorId = authorId;
            Tags = tags ?? new List<string>();
            Price = 0; 
            Status = TourStatus.DRAFT; 
            KeyPoints = new List<KeyPoint>(); 
        }

        public void AddKeyPoint(KeyPoint keyPoint)
        {
            if (keyPoint == null) throw new ArgumentNullException("Invalid keypoint.");
            if (Status == TourStatus.ARCHIVED) throw new InvalidOperationException("Cannot modify key point of archived tours.");

            KeyPoints ??= new List<KeyPoint>();
            if (KeyPoints.Count > 0)
            {
                var lastPoint = KeyPoints.Last();
                DistanceInKm += HaversineDistanceInKm(lastPoint.Latitude, lastPoint.Longitude, keyPoint.Latitude, keyPoint.Longitude);
            }


            KeyPoints.Add(keyPoint);
        }

        private static double ToRadians(double angle) => Math.PI * angle / 180.0;

        private static double HaversineDistanceInKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371.0;
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        public void Publish(long authorId)
        {
            if (Status != TourStatus.DRAFT)
                throw new InvalidOperationException("Only draft tours can be published.");

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || Price < 0)
                throw new InvalidOperationException("Tour must have all basic fields filled.");

            if (Tags == null || Tags.Count == 0)
                throw new InvalidOperationException("Tour must have at least one tag.");

            if (KeyPoints == null || KeyPoints.Count < 2)
                throw new InvalidOperationException("Tour must have at least two key points.");

            if (Durations == null || Durations.Count == 0)
                throw new InvalidOperationException("Tour must have at least one duration defined (e.g., walking, cycling).");

            Status = TourStatus.CONFIRMED;
            PublishedDateTime = DateTime.UtcNow;
        }

        public void Archive()
        {
            if (Status != TourStatus.CONFIRMED) throw new InvalidOperationException("Only published tours can be archived.");

            Status = TourStatus.ARCHIVED;
            ArchivedDateTime = DateTime.UtcNow;
        }

        public void Activate()
        {
            if (Status != TourStatus.ARCHIVED) throw new InvalidOperationException("Only archived tours can be reactiveted.");

            Status = TourStatus.CONFIRMED;
            ArchivedDateTime = DateTime.UtcNow;
        }
    }
}
