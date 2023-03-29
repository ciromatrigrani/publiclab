using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Dto;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Exceptions;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Mapping;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Application.Services;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Repository;
using MatrigraniCiro.DotNet6.MinimalApi.IsraCardChallenge.Login.Repository.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

/*
 * General config
 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PersonalInfoContext>(opt => opt.UseInMemoryDatabase("flight_mesh_db"));
builder.Services.AddAutoMapper(mapper => mapper.AddMaps(typeof(Mapping).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Ciro Matrigrani Minimal Api IsraCard Challenge Login",
        Description = "DotNet Core Minimal API for Login JWT",
        Contact = new OpenApiContact
        {
            Name = "Ciro Matrigrani",
            Email = "ciromatrigrani@gmail.com",
            Url = new Uri("https://sites.google.com/site/cmatripgita/")
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
   });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])),

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,

        };
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyHeader())
);

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<PersonalInfoContext>();
        ContextInitialiser.Run(context);
    }
}
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

/*
 * Dot NET 6 Minimal API Login Controller
 */

app.MapPost("api/v1/auth", [AllowAnonymous] async ([FromBody] LoginRequest loginRequest, [FromServices] IAuthService authService) =>
{
    try
    {
        var authReponse = await authService.Login(loginRequest);
        return Results.Ok(authReponse);
    }
    catch { return Results.Unauthorized(); }
});

app.MapGet("api/v1/auth", [Authorize] async ([FromServices] IAuthService authService) => Results.Ok(await authService.GetAuthData()));

app.MapDelete("api/v1/auth/{Id}", [Authorize] async ([FromRoute] Guid id, [FromServices] IAuthService authService) =>
        await authService.DeleteAuthData(id) ? Results.NoContent() : Results.NotFound(id));

app.Run();