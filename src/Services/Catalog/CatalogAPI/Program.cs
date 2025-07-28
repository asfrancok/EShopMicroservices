using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using CatalogAPI.Data;
using HealthChecks.UI.Client;
using System.Reflection;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(new DependencyContextAssemblyCustomCatalog());

builder.Services.AddMediatR(cnf =>
{
    cnf.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cnf.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cnf.RegisterServicesFromAssembly(assembly);   
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(opts => { });

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
        return new []{ typeof(Program).Assembly };
    }
}