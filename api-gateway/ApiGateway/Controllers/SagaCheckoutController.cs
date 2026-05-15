using ApiGateway.Protos;
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.ClientFactory;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/gateway/checkout-saga")]
    public class SagaCheckoutController : ControllerBase
    {
        private readonly CheckoutSagaGrpcService.CheckoutSagaGrpcServiceClient _purchaseClient;
        private readonly CheckoutSagaGrpcService.CheckoutSagaGrpcServiceClient _toursClient;

        public SagaCheckoutController(GrpcClientFactory clientFactory)
        {
            _purchaseClient = clientFactory.CreateClient<CheckoutSagaGrpcService.CheckoutSagaGrpcServiceClient>("PurchaseSagaClient");
            _toursClient = clientFactory.CreateClient<CheckoutSagaGrpcService.CheckoutSagaGrpcServiceClient>("ToursSagaClient");
        }

        [HttpPost("{touristId}")]
        public async Task<IActionResult> ExecuteSaga(long touristId)
        {
            var purchaseResponse = await _purchaseClient.StartCheckoutAsync(new SagaCheckoutRequest { TouristId = touristId });

            if (!purchaseResponse.Success)
            {
                return BadRequest(new { message = "Step 1 (Purchase) failed", detail = purchaseResponse.Message });
            }

            try
            {
                var validationRequest = new SagaRollbackRequest { TouristId = touristId };
                foreach (var id in purchaseResponse.PurchasedTourIds)
                {
                    validationRequest.TourIds.Add(id);
                }

                var validationResponse = await _toursClient.RollbackCheckoutAsync(validationRequest);

                if (!validationResponse.Success)
                {
                    throw new Exception("One or more tours are not available (Archived or Draft).");
                }

                return Ok(new { message = "SAGA Completed Successfully!", tokens = purchaseResponse.PurchasedTourIds });
            }
            catch (Exception ex)
            {
                var rollbackRequest = new SagaRollbackRequest { TouristId = touristId };
                foreach (var id in purchaseResponse.PurchasedTourIds)
                {
                    rollbackRequest.TourIds.Add(id);
                }

                await _purchaseClient.RollbackCheckoutAsync(rollbackRequest);

                return StatusCode(500, new { message = "SAGA Rollback executed.", reason = ex.Message });
            }
        }
    }
}