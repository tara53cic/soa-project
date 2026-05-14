using Grpc.Core;
using PurchaseService.Protos;
using PurchaseService.Services.Interfaces;
using PurchaseService.Repositories.Interfaces;

namespace PurchaseService.Services
{
    public class CheckoutSagaGrpcServiceImpl : CheckoutSagaGrpcService.CheckoutSagaGrpcServiceBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly IShoppingCartRepository _repository;

        public CheckoutSagaGrpcServiceImpl(IShoppingCartService cartService, IShoppingCartRepository repository)
        {
            _cartService = cartService;
            _repository = repository;
        }

        public override async Task<SagaCheckoutResponse> StartCheckout(SagaCheckoutRequest request, ServerCallContext context)
        {
            try
            {
                var tokens = _cartService.Checkout(request.TouristId);

                var response = new SagaCheckoutResponse
                {
                    Success = true,
                    Message = "Tokens successfully created."
                };

                foreach (var t in tokens)
                {
                    response.PurchasedTourIds.Add(t.TourId);
                }

                return response;
            }
            catch (Exception ex)
            {
                return new SagaCheckoutResponse { Success = false, Message = ex.Message };
            }
        }

        public override async Task<SagaRollbackResponse> RollbackCheckout(SagaRollbackRequest request, ServerCallContext context)
        {
            try
            {
                foreach (var tourId in request.TourIds)
                {
                    _repository.DeleteToken(request.TouristId, tourId);
                }
                return new SagaRollbackResponse { Success = true };
            }
            catch (Exception)
            {
                return new SagaRollbackResponse { Success = false };
            }
        }
    }
}