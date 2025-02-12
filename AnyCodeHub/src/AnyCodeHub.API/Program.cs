using AnyCodeHub.API.Middlewares;
using AnyCodeHub.Application.DependencyInjections.Extensions;
using AnyCodeHub.Persistence.DependencyInjections.Extensions;
using AnyCodeHub.Persistence.DependencyInjections.Options;
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

builder.Services.AddPostgreSql();
builder.Services.AddInterceptors();
builder.Services.ConfigurePostgreSqlRetryOptions(builder.Configuration.GetSection(nameof(PostgreSqlRetryOptions)));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
