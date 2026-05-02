using PurchaseService.Models;

namespace PurchaseService.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetCartByTouristId(long touristId);
        ShoppingCart CreateCart(ShoppingCart cart);
        void UpdateCart(ShoppingCart cart);

        bool HasPurchasedTour(long touristId, long tourId);
        void SaveTokens(List<TourPurchaseToken> tokens);
        void RemoveOrderItem(OrderItem item);
    }
}
