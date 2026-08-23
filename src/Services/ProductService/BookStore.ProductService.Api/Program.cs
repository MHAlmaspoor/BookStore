// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName
// ("GetWeatherForecast");

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

//=====>  .net 9 Template
// var builder = WebApplication.CreateBuilder(args);

// // builder.Services.AddEndpointsApiExplorer();
// // builder.Services.AddSwaggerGen();
// builder.Services.AddPresentation();
// builder.Services.AddApplication();
// builder.Services.AddInfrastructure(builder.Configuration);
// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.MapGet("/weatherforecast", () =>
// {
//     return "Hello";
// });

// app.Run();

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


// var builder=WebApplication.CreateBuilder(args);
// builder.Services.AddApplication();
// builder.Services.AddInfrastructure(builder.Configuration);
// builder.Services.AddPresentation();
// builder.Services.AddControllers();
// builder.Services.AddHostedService<OutboxProcessor>();

// builder.Services.AddSingleton<
//     IAuthorizationPolicyProvider,
//     PermissionPolicyProvider>();

// builder.Services.AddSingleton<
//     IAuthorizationHandler,
//     PermissionAuthorizationHandler>();

// var jwtOptions = builder.Configuration
//     .GetSection(JwtOptions.SectionName)
//     .Get<JwtOptions>()
//     ?? throw new InvalidOperationException(
//         "JWT configuration is missing.");

// builder.Services
//     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidIssuer = jwtOptions.Issuer,

//             ValidateAudience = true,
//             ValidAudience = jwtOptions.Audience,

//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),

//             ValidateLifetime = true,
//             ClockSkew = TimeSpan.Zero
//         };

//         options.Events = new JwtBearerEvents
//         {
//             OnMessageReceived = context =>
//             {
//                 Console.WriteLine(
//                     ">>> JwtBearer: OnMessageReceived");

//                 Console.WriteLine(
//                     $"Token exists: {!string.IsNullOrWhiteSpace(context.Token)}");

//                 return Task.CompletedTask;
//             },

//             OnAuthenticationFailed = context =>
//             {
//                 Console.WriteLine(
//                     $">>> JwtBearer FAILED: {context.Exception}");

//                 return Task.CompletedTask;
//             },

//             OnTokenValidated = context =>
//             {
//                 Console.WriteLine(
//                     ">>> JwtBearer VALIDATED");

//                 return Task.CompletedTask;
//             }
//         };
//     });


// builder.Services.AddSwaggerGen(options =>
// {
//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Description = "JWT Authorization header using the Bearer scheme.",
//         Name = "Authorization",
//         In = ParameterLocation.Header,
//         Type = SecuritySchemeType.Http,
//         Scheme = "Bearer"
//     });
// });


// builder.Services.AddAuthorization();
// var app=builder.Build();
// app.UsePresentation();
// app.MapControllers();

// app.UseAuthentication();

// app.UseAuthorization();
// app.Run();

var builder = WebApplication.CreateBuilder(args);

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


var app = builder.Build();

app.UsePresentation();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
