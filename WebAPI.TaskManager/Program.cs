using TaskManager.Infrastructure.Persistence;
using TaskManager.Core.Application;
using System.Security.Cryptography;
using TaskManager.Infrastructure.Security;
using Microsoft.OpenApi.Models;
using WebAPI.TaskManager.Configuration;

var builder = WebApplication.CreateBuilder(args);

byte[] secretKey = new byte[32];
using var rng = RandomNumberGenerator.Create();
rng.GetBytes(secretKey);
string secretKeyString = Convert.ToBase64String(secretKey);

// Add the secret key to the configuration
builder.Configuration["Authentication:SecretKey"] = secretKeyString;

// Add services to the container.
builder.Services.AddServicesConfiguration(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddSecutiryLayer(builder.Configuration);
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo() { Title = "TASK API", Version = "v1" });

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingresa JWT con Bearer dentro del campo.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
           },
           Array.Empty<string>()
        }
     });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task v1");
    });
}

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
