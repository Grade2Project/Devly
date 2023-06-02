using System.Text;
using System.Text.Json.Serialization;
using Devly.Configs;
using Devly.Database.Context;
using Devly.Database.Extensions.DI;
using Devly.Extensions;
using Devly.Helpers;
using Devly.Services;
using Devly.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:63342");
            policy.AllowAnyHeader();
        });
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("UserPolicy", p =>
    {
        p.Requirements.Add(new DenyAnonymousAuthorizationRequirement());
        p.RequireRole("User");
    });
    o.AddPolicy("CompanyPolicy", p =>
    {
        p.Requirements.Add(new DenyAnonymousAuthorizationRequirement());
        p.RequireRole("Company");
    });
});


services.AddMemoryCache();
services.AddSingleton<Random, Random>();
services.AddControllers()
    .AddJsonOptions(o => { o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
services.AddSwaggerGen(s => s.SwaggerDoc("v1", new OpenApiInfo
{
    Title = "Devly", Version = "v1"
}));
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services
    .AddNpgsqlDbContext<DevlyDbContext>(config.GetConnectionString("Postgres"))
    .AddDatabase();

services.AddConfig<AuthConfig>(config.GetRequiredSection("Auth"));

services.AddSingleton<IPasswordHasher, ShaPasswordHasher>();
services.AddSingleton<IIdentityService, IdentityService>();
services.AddHelpers();

var app = builder.Build();

app.UseCors(myAllowSpecificOrigins);
app.UseSwagger().UseSwaggerUI();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();