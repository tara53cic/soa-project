using ApiGateway.Protos;
using Microsoft.AspNetCore.Mvc;
using Grpc.Net.ClientFactory;
using System;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("saga/block-user")]
    public class SagaBlockController : ControllerBase
    {
        private readonly BlockSagaGrpcService.BlockSagaGrpcServiceClient _stakeholdersClient;
        private readonly BlockSagaGrpcService.BlockSagaGrpcServiceClient _followClient;

        public SagaBlockController(GrpcClientFactory clientFactory)
        {
            _stakeholdersClient = clientFactory.CreateClient<BlockSagaGrpcService.BlockSagaGrpcServiceClient>("StakeholdersSagaClient");
            _followClient = clientFactory.CreateClient<BlockSagaGrpcService.BlockSagaGrpcServiceClient>("FollowSagaClient");
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> ExecuteBlockSaga([FromRoute] long userId, [FromQuery] bool isBlocked)
        {
            var stakeholdersResponse = await _stakeholdersClient.SyncUserBlockStatusAsync(new BlockStatusRequest
            {
                UserId = userId,
                IsBlocked = isBlocked
            });

            if (!stakeholdersResponse.Success)
            {
                return BadRequest(new { message = "Step 1 (Stakeholders - Postgres) failed", detail = stakeholdersResponse.Message });
            }

            try
            {
                var followResponse = await _followClient.SyncUserBlockStatusAsync(new BlockStatusRequest
                {
                    UserId = userId,
                    IsBlocked = isBlocked
                });

                if (!followResponse.Success)
                {
                    throw new Exception("Go service failed to update Neo4j.");
                }

                return Ok(new { message = "Block SAGA Completed Successfully!" });
            }
            catch (Exception ex)
            {
                await _stakeholdersClient.SyncUserBlockStatusAsync(new BlockStatusRequest
                {
                    UserId = userId,
                    IsBlocked = !isBlocked 
                });

                return StatusCode(500, new { message = "SAGA Rollback executed.", reason = ex.Message });
            }
        }
    }
}