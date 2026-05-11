using Microsoft.AspNetCore.Mvc;
using ApiGateway.Protos;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("gateway/purchase")]
    public class GatewayPurchaseController : ControllerBase
    {
        private readonly PurchaseGrpc.PurchaseGrpcClient _grpcClient;

        public GatewayPurchaseController(PurchaseGrpc.PurchaseGrpcClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpPost("checkout/{touristId}")]
public async Task<IActionResult> RemoteCheckout(long touristId)
{
    try 
    {
        var request = new CheckoutRequest { TouristId = touristId };
        var response = await _grpcClient.CheckoutAsync(request);
        return Ok(response.Tokens);
    }
    catch (Exception ex)
    {
        
        return StatusCode(500, new { error = ex.Message, detail = ex.InnerException?.Message });
    }
}
    }
}
