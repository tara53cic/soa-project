namespace PurchaseService.Models
{
    public class OrderItem
    {
        public long Id { get; set; }
        public long TourId { get; set; }
        public string TourName { get; set; }
        public double Price { get; set; }
        public long ShoppingCartId { get; set; }
    }
}
