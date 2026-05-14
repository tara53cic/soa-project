using Microsoft.AspNetCore.Mvc;
using ApiGateway.Protos;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("gateway/tours")]
    public class GatewayToursController : ControllerBase
    {
        private readonly ToursGrpc.ToursGrpcClient _grpcClient;

        public GatewayToursController(ToursGrpc.ToursGrpcClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTour(long id)
        {
            try
            {
                var request = new TourRequest
                {
                    Id = id
                };

                var response = await _grpcClient.GetTourByIdAsync(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}