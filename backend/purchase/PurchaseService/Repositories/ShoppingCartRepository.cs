using PurchaseService.Data;
using PurchaseService.Models;
using PurchaseService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PurchaseService.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly PurchaseDbContext _context;

        public ShoppingCartRepository(PurchaseDbContext context)
        {
            _context = context;
        }

        public ShoppingCart GetCartByTouristId(long touristId)
        {
            return _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefault(c => c.TouristId == touristId);
        }

        public ShoppingCart CreateCart(ShoppingCart cart)
        {
            _context.ShoppingCarts.Add(cart);
            _context.SaveChanges();
            return cart;
        }

        public void UpdateCart(ShoppingCart cart)
        {
            _context.ShoppingCarts.Update(cart);
            _context.SaveChanges();
        }

        public void SaveTokens(List<TourPurchaseToken> tokens)
        {
            _context.OrderTokens.AddRange(tokens);
            _context.SaveChanges();
        }

        public bool HasPurchasedTour(long touristId, long tourId)
        {
            return _context.OrderTokens.Any(t => t.TouristId == touristId && t.TourId == tourId);
        }

        public void RemoveOrderItem(OrderItem item)
        {
            _context.OrderItems.Remove(item);
            _context.SaveChanges();
        }
    }
}
