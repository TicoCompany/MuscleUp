using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using MuscleUp.Dominio;
using MuscleUp.Dominio.Cloudinary;
using MuscleUp.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddDataBaseConfiguration();
builder.AddSessionConfig();
builder.Services.AddControllersWithViews();
builder.Services.AddDominioServices();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddSingleton<Cloudinary>(provider =>
{
    var config = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
    return new Cloudinary(account);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
 app.UseAuthentication();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
