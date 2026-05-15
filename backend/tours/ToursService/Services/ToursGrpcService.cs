using Grpc.Core;
using ToursService.Protos;
using ToursService.Services.Interfaces;

public class ToursGrpcService : ToursGrpc.ToursGrpcBase
{
    private readonly ITourService _tourService;

    public ToursGrpcService(ITourService tourService)
    {
        _tourService = tourService;
    }
    public override async Task<TourResponse> GetTourById(
        TourRequest request,
        ServerCallContext context)
    {
        var tour = _tourService.GetById(request.Id);

        if (tour == null)
            throw new RpcException(new Status(StatusCode.NotFound, "Tour not found"));

        return new TourResponse
        {
            Id = tour.Id,
            Name = tour.Name,
            Description = tour.Description
        };
    }
}