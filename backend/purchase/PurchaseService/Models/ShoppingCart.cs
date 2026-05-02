namespace PurchaseService.Models
{
    public class ShoppingCart
    {
        public long Id { get; set; }
        public long TouristId { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public double TotalPrice { get; set; }

        public void CalculateTotal()
        {
            TotalPrice = Items.Sum(item => item.Price);
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
            CalculateTotal();
        }
        public void RemoveItem(OrderItem item) { 
        Items.Remove(item);
        CalculateTotal();
        }
    }
}
