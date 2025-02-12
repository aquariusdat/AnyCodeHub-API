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

#region Application
// Api configurations
builder.Services.AddControllers().AddApplicationPart(AnyCodeHub.Presentation.AssemblyReference.Assembly);

// Add configurations
builder.Services.AddMediatRApplication();
builder.Services.AddAutoMapperApplication();
#endregion Application

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
