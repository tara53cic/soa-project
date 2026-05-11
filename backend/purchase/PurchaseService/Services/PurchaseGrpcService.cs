using Grpc.Core;
using PurchaseService.Services.Interfaces;
using PurchaseService.Protos;

namespace PurchaseService.Services
{
    public class PurchaseGrpcService : PurchaseGrpc.PurchaseGrpcBase
    {
        private readonly IShoppingCartService _cartService;
        public PurchaseGrpcService(IShoppingCartService cartService) => _cartService = cartService;

        public override Task<HasPurchasedResponse> HasPurchasedTour(HasPurchasedRequest request, ServerCallContext context)
        {
            var result = _cartService.hasPurchasedTour(request.TouristId, request.TourId);
            return Task.FromResult(new HasPurchasedResponse { HasPurchased = result });
        }

        public override Task<CheckoutResponse> Checkout(CheckoutRequest request, ServerCallContext context)
        {
            try
            {
                var resultTokens = _cartService.Checkout(request.TouristId);

                var response = new CheckoutResponse();
                foreach (var token in resultTokens)
                {
                    response.Tokens.Add(new TourPurchaseTokenDto
                    {
                        Id = token.Id,
                        TourId = token.TourId
                    });
                }

                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
    }
}
