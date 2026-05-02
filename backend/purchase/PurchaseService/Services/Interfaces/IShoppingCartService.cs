using PurchaseService.DTOs;

namespace PurchaseService.Services.Interfaces
{
    public interface IShoppingCartService
    {
        ShoppingCartDto GetCart(long touristId);
        ShoppingCartDto AddItemToCart(long touristId, OrderItemDto itemDto);
        ShoppingCartDto RemoveItemFromCart(long touristId, long tourId);
        List<TourPurchaseTokenDto> Checkout(long touristId);

        bool hasPurchasedTour(long touristId, long tourId);
    }
}
