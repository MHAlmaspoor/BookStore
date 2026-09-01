using BookStore.ProductService.Api.Extensions;
using BookStore.ProductService.Application;
using BookStore.BuildingBlocks.Persistence.Outbox;
using BookStore.ProductService.Infrastructure;
using BookStore.BuildingBlocks.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BookStore.ProductService.Infrastructure.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using BookStore.BuildingBlocks.Infrastructure.DependencyInjection;
using BookStore.ProductService.Infrastructure.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using BookStore.ProductService.Api.Middleware;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: "BookStore.ProductService"))
    .WithTracing(tracking =>
    {
        tracking
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddSource("BookStore.ProductService.Redis")
            .AddSource("BookStore.BuildingBlocks.Outbox")
            .AddSource("BookStore.BuildingBlocks.Messaging")
            .AddConsoleExporter()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:4317");
            });
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation();
builder.Services.AddControllers();
builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.AddSingleton<
    IAuthorizationPolicyProvider,
    PermissionPolicyProvider>();

builder.Services.AddSingleton<
    IAuthorizationHandler,
    PermissionAuthorizationHandler>();

var jwtOptions = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()
    ?? throw new InvalidOperationException(
        "JWT configuration is missing.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine(
                    ">>> JwtBearer: OnMessageReceived");

                Console.WriteLine(
                    $"Token exists: {!string.IsNullOrWhiteSpace(context.Token)}");

                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(
                    $">>> JwtBearer FAILED: {context.Exception}");

                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine(
                    ">>> JwtBearer VALIDATED");

                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });
});



builder.Services.AddAuthorization();

builder.Services.AddRabbitMq(builder.Configuration);

builder.Services
    .AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("ProductDatabase")!,
        name: "postgres",
        tags: ["ready"])
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!,
        name: "redis",
        tags: ["redis"])
    .AddCheck<RabbitMqHealthCheck>("rabbitmq", failureStatus: HealthStatus.Degraded, tags: ["ready"]);

builder.Logging.Configure(options =>
{
   options.ActivityTrackingOptions =
        ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId | ActivityTrackingOptions.ParentId;
});

builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
});

var app = builder.Build();

app.UsePresentation();
app.MapHealthChecks("/health");
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();




app.Run();
