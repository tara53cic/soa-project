namespace PurchaseService.Models
{
    public class TourPurchaseToken
    {
        public long Id { get; set; }
        public long TouristId { get; set; }
        public long TourId { get; set; }
        public DateTime PurchaseTime { get; set; }
    }
}
