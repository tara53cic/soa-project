using Grpc.Core;
using ToursService.Protos;
using ToursService.Repositories.Interfaces;

namespace ToursService.Services
{
    public class ToursSagaGrpcServiceImpl : CheckoutSagaGrpcService.CheckoutSagaGrpcServiceBase
    {
        private readonly ITourRepository _repository;

        public ToursSagaGrpcServiceImpl(ITourRepository repository)
        {
            _repository = repository;
        }

        public override async Task<SagaRollbackResponse> RollbackCheckout(SagaRollbackRequest request, ServerCallContext context)
        {
            try
            {
                foreach (var tourId in request.TourIds)
                {
                    var tour = _repository.GetById(tourId);

                    if (tour == null || (int)tour.Status != 1)
                    {
                        return new SagaRollbackResponse { Success = false };
                    }
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