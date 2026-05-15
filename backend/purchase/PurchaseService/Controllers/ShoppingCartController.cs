using Microsoft.AspNetCore.Mvc;
using PurchaseService.DTOs;
using PurchaseService.Services.Interfaces;

namespace PurchaseService.Controllers
{
    [ApiController]
    [Route("api/shopping-cart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;

        public ShoppingCartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{touristId}")]
        public ActionResult<ShoppingCartDto> GetCart(long touristId)
        {
            var cart = _cartService.GetCart(touristId);
            return Ok(cart);
        }

        [HttpPost("{touristId}/add")]
        public ActionResult<ShoppingCartDto> AddToCart(long touristId, [FromBody] OrderItemDto itemDto)
        {
            try
            {
                var cart = _cartService.AddItemToCart(touristId, itemDto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{touristId}/remove/{tourId}")]
        public ActionResult<ShoppingCartDto> RemoveFromCart(long touristId, long tourId)
        {
            var cart = _cartService.RemoveItemFromCart(touristId, tourId);
            return Ok(cart);
        }

        [HttpPost("{touristId}/checkout")]
        public ActionResult<List<TourPurchaseTokenDto>> Checkout(long touristId)
        {
            try
            {
                var tokens = _cartService.Checkout(touristId);
                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{touristId}/has-purchased/{tourId}")]
        public ActionResult<bool> HasPurchasedTour(long touristId, long tourId)
        {
            var hasPurchased = _cartService.hasPurchasedTour(touristId, tourId);
            return Ok(hasPurchased);}
    }
}
