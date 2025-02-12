using AnyCodeHub.API.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
Log.Logger = new LoggerConfiguration().ReadFrom
                .Configuration(builder.Configuration)
                .CreateLogger();
builder.Logging.ClearProviders().AddSerilog();
builder.Host.UseSerilog();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
