using System.Text.Json.Serialization;
using MuscleUp.Api.Configurations;
using MuscleUp.Dominio;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddCors();
services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerConfiguration();
services.AddHttpClient();
builder.Services.AddSwaggerGen();
builder.Services.AddDominioServices();

services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
    });

builder.AddSessionConfig();

var app = builder.Build();

app.UseSwaggerConfiguration();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
