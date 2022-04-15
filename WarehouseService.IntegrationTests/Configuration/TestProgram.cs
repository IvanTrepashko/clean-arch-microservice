using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WarehouseService.API.Mappings;
using WarehouseService.API.Options;
using WarehouseService.Application;
using WarehouseService.Domain;
using WarehouseService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(DefaultProfile));
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));

// Domain dependencies
builder.Services.RegisterDomainDependencies();

// Infrastructure dependencies

// Application dependencies
builder.Services.AddRequestHandlers();

builder.Services.AddMassTransitWithMongoDb(builder.Configuration);

builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();

public partial class TestProgram { }