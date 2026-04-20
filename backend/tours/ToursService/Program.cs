using Microsoft.EntityFrameworkCore;
using ToursService.Data; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToursDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddScoped<ToursService.Repositories.Interfaces.ITourRepository, ToursService.Repositories.TourRepository>();
builder.Services.AddScoped<ToursService.Services.Interfaces.ITourService, ToursService.Services.TourService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ToursDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();