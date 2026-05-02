namespace PurchaseService.DTOs
{
    public class TourPurchaseTokenDto
    {
        public long Id { get; set; }
        public long TourId { get; set; }
        public DateTime PurchaseTime { get; set; }
    }
}
