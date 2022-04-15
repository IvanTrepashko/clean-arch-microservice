using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WarehouseService.API.Mappings;
using WarehouseService.API.Options;
using WarehouseService.Application;
using WarehouseService.Domain;
using WarehouseService.Infrastructure;
using WarehouseService.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(DefaultProfile));
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMqOptions"));
builder.Services.Configure<MongoDbOptions>(builder.Configuration.GetSection("MongoDbOptions"));


// Domain dependencies
builder.Services.RegisterDomainDependencies();

// Infrastructure dependencies
builder.Services.AddInfrastructureDependencied(builder.Configuration.GetSection("MongoDbOptions"));

// Application dependencies
builder.Services.AddRequestHandlers();

builder.Services.AddMassTransitWithMongoDb(builder.Configuration);

builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace WarehouseService.API
{
    public partial class Program { }
}