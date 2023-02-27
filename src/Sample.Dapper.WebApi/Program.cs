using Microsoft.Extensions.DependencyInjection.Extensions;
using Sample.Dapper.Persistence.Interfaces;
using Sample.Dapper.Persistence.Repositories;
using Sample.Dapper.WebApi.Core.Extensions;
using Serilog;

try
{
	var builder = WebApplication.CreateBuilder(args);
	builder.AddSerilog("API Dapper");

	builder.Services.AddRouting(options => options.LowercaseUrls = true);
	builder.Services.AddControllers();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	builder.Services.AddScoped<IUserRepository, UserRepository>(_ => new UserRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
	builder.Services.AddSingleton<DatabaseExtensions>();
	builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IStartupFilter, MigrationManager>());

	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	app.UseAuthorization();
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