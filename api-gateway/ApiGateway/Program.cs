using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ApiGateway.Protos;
using System.Net.Http;
using System;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("GatewayCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddGrpcClient<PurchaseGrpc.PurchaseGrpcClient>(o =>
{
    o.Address = new Uri("http://purchase_service:44394");
});

builder.Services.AddGrpcClient<ToursGrpc.ToursGrpcClient>(o =>
{
    o.Address = new Uri("http://localhost:7195");
});

builder.Services.AddGrpcClient<BlogGrpcService.BlogGrpcServiceClient>(o => {
    o.Address = new Uri("http://blog_service:44307");
});

var app = builder.Build();

app.UseRouting();
app.UseCors("GatewayCorsPolicy");

app.MapControllers();

app.MapReverseProxy();

app.Run();
