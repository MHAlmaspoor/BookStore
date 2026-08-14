using BookStore.IdentityService.Application.Command.Register;
using FluentValidation;
using BookStore.IdentityService.Application.Behaviors;
using BookStore.IdentityService.Api.ExceptionHandling;
using BookStore.IdentityService.Api.Endpoints.Authentication;
using BookStore.IdentityService.Infrastructure.DependencyInjection;
using BookStore.IdentityService.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using BookStore.IdentityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using BookStore.IdentityService.Infrastructure.Authorization;
using BookStore.BuildingBlocks.Infrastructure.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Swagger

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
       Name = "Authorization",
       Type = SecuritySchemeType.Http,
       Scheme = "bearer",
       BearerFormat = "JWT",
       In = ParameterLocation.Header,
       Description = "Enter JWT Token"

    });
    // options.AddSecurityRequirement(
    //     new OpenApiSecurityRequirement
    //     {
    //         {
    //             new OpenApiSecurityScheme
    //             {
    //                 Reference = new OpenApiReference
    //                 {
    //                     Type = ReferenceType.SecurityScheme,
    //                     Id = "Bearer"
    //                 }
    //             },
    //             Array.Empty<string>()
    //         }
    //     });
});

//builder.Services.AddSwaggerGen();
///

builder.Services.AddValidatorsFromAssembly(typeof(RegisterCommandValidator).Assembly);
builder.Services.AddMediatR(cfg =>
{
   cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
   cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddInfrastructure(builder.Configuration);


var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
    });


builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddAuthorization();
builder.Services.AddRabbitMq(builder.Configuration);
var app = builder.Build();

await app.Services.SeedInfrastructureAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();

    app.UseSwagger();

    app.UseSwaggerUI();
    //app.MapOpenApi();

}

app.UseHttpsRedirection();

app.MapRegisterEndpoint();

app.MapLoginEndpoint();

app.MapRefreshEndpoint();

app.MapMeEndpoint();

app.MapLogoutEndpoint();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

