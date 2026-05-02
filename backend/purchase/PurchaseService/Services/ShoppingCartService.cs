using AutoMapper;
using PurchaseService.DTOs;
using PurchaseService.Models;
using PurchaseService.Services.Interfaces;
using PurchaseService.Repositories.Interfaces;

namespace PurchaseService.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _repository;
        private readonly IMapper _mapper;

        public ShoppingCartService(IShoppingCartRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ShoppingCartDto GetCart(long touristId)
        {
            var cart = GetOrCreateCart(touristId);
            return _mapper.Map<ShoppingCartDto>(cart);
        }

        public bool hasPurchasedTour(long touristId, long tourId)
        {
            return _repository.HasPurchasedTour(touristId, tourId);
        }

        public ShoppingCartDto AddItemToCart(long touristId, OrderItemDto itemDto)
        {
            var cart = GetOrCreateCart(touristId);


            if (_repository.HasPurchasedTour(touristId, itemDto.TourId))
                throw new InvalidOperationException("You have already purchased this tour.");

            var item = _mapper.Map<OrderItem>(itemDto);
            cart.AddItem(item); 

            _repository.UpdateCart(cart);
            return _mapper.Map<ShoppingCartDto>(cart);
        }

        public ShoppingCartDto RemoveItemFromCart(long touristId, long tourId)
        {
            var cart = GetOrCreateCart(touristId);
            var itemToRemove = cart.Items.FirstOrDefault(i => i.TourId == tourId);

            if (itemToRemove != null)
            {
                cart.RemoveItem(itemToRemove);
                _repository.RemoveOrderItem(itemToRemove); 
                _repository.UpdateCart(cart);
            }

            return _mapper.Map<ShoppingCartDto>(cart);
        }

        public List<TourPurchaseTokenDto> Checkout(long touristId)
        {
            var cart = GetOrCreateCart(touristId);

            if (!cart.Items.Any())
                throw new InvalidOperationException("Cannot checkout an empty cart.");

            var tokens = new List<TourPurchaseToken>();
            foreach (var item in cart.Items)
            {
                tokens.Add(new TourPurchaseToken
                {
                    TouristId = touristId,
                    TourId = item.TourId,
                    PurchaseTime = DateTime.UtcNow
                });
            }

            _repository.SaveTokens(tokens);

            foreach (var item in cart.Items.ToList()) 
            {
                _repository.RemoveOrderItem(item);
            }
            cart.Items.Clear();
            cart.CalculateTotal(); 
            _repository.UpdateCart(cart);

            return _mapper.Map<List<TourPurchaseTokenDto>>(tokens);
        }

        private ShoppingCart GetOrCreateCart(long touristId)
        {
            var cart = _repository.GetCartByTouristId(touristId);
            if (cart == null)
            {
                cart = new ShoppingCart { TouristId = touristId, TotalPrice = 0 };
                _repository.CreateCart(cart);
            }
            return cart;
        }
    }
}
