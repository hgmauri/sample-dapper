using System.Reflection;
using DapperExtensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.Dapper.Persistence.Entities;
using Sample.Dapper.Persistence.Interfaces;
using Sample.Dapper.Persistence.Repositories;
using Sample.Dapper.WebApi.Core.Extensions;
using Serilog;

SerilogExtensions.AddSerilog("API Sample Dapper");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(Log.Logger);

    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Sample.Dapper.WebApi", Version = "v1" });
    });

    builder.Services.AddScoped<IUserRepository, UserRepository>(_ =>
    {
        return new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    builder.Services.AddSingleton<DatabaseExtensions>();
    builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupFilter, MigrationManager>());

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample.Dapper.WebApi v1"));
    }

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}