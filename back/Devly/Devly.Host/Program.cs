using System.Text.Json.Serialization;
using Devly.Configs;
using Devly.Database.Context;
using Devly.Database.Extensions.DI;
using Devly.Extensions;
using Devly.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
services.AddSwaggerGen(s => s.SwaggerDoc("v1", new OpenApiInfo()
{
    Title = "Devly", Version = "v1"
}));

services
    .AddNpgsqlDbContext<DevlyDbContext>(config.GetConnectionString("Postgres"))
    .AddDatabase();

services.AddConfig<AuthConfig>(config.GetRequiredSection("Auth"));

services.AddSingleton<IPasswordHasher, ShaPasswordHasher>();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();
app.UseRouting();
app.MapControllers();
app.Run();
