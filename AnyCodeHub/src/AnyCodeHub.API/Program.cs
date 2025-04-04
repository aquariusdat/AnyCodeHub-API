using AnyCodeHub.API.DependencyInjection.Extensions;
using AnyCodeHub.API.Middlewares;
using AnyCodeHub.Application.DependencyInjections.Extensions;
using AnyCodeHub.Infrastructure.DependencyInjections.Extensions;
using AnyCodeHub.Persistence;
using AnyCodeHub.Persistence.DependencyInjections.Extensions;
using AnyCodeHub.Persistence.DependencyInjections.Options;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddPostgreSql();
builder.Services.AddInterceptors();
builder.Services.ConfigurePostgreSqlRetryOptions(builder.Configuration.GetSection(nameof(PostgreSqlRetryOptions)));

#region Application
// Api configurations
builder.Services.AddControllers().AddApplicationPart(AnyCodeHub.Presentation.AssemblyReference.Assembly);

// Add configurations
builder.Services.AddMediatRApplication();
builder.Services.AddAutoMapperApplication();
#endregion Application

#region Infrastructure
builder.Services.AddRedisInfrastructure(builder.Configuration);
// Add dapper configurations
//builder.Services.AddDapperInfrastructure();
// Add Jwt configurations
builder.Services.AddJwtInfrastructure(builder.Configuration);
// Add Masstransit Producer
//builder.Services.AddProducerInfrastructure(builder.Configuration);
//builder.Services.AddQuartzInfrastructure();
#endregion Infrastructure

// Add Swagger configrations
builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwagger();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddPostgreSql();
builder.Services.AddInterceptors();
builder.Services.ConfigurePostgreSqlRetryOptions(builder.Configuration.GetSection(nameof(PostgreSqlRetryOptions)));

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
    });
}

app.UseAuthorization();

app.MapControllers();

if (!builder.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
    }
}

app.Run();
