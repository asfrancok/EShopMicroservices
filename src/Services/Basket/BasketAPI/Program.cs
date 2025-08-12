using BasketAPI.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using DiscountGrpc;
using HealthChecks.UI.Client;
using System.Reflection;
using BuildingBlocks.Messaging.MassTransit;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(new DependencyContextAssemblyCustomCatalog());

builder.Services.AddMediatR(cnf =>
{
    cnf.RegisterServicesFromAssembly(assembly);
    cnf.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cnf.AddOpenBehavior(typeof(LoggingBehavior<,>));
});


builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();


builder.Services.AddStackExchangeRedisCache(cache =>
{
    cache.Configuration = builder.Configuration.GetConnectionString("Redis");
});


builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler //Only for development env, in prod there should be a certificate
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

// No assembly for publisher side
builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(opt => { });

app.UseHealthChecks("/health",
    new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();


public class DependencyContextAssemblyCustomCatalog : DependencyContextAssemblyCatalog
{
    public override IReadOnlyCollection<Assembly> GetAssemblies()
    {
        return new[] { typeof(Program).Assembly };
    }
}
