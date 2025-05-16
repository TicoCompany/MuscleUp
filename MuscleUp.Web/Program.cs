using MuscleUp.Web.Configurations;
using MuscleUp.Dominio;

var builder = WebApplication.CreateBuilder(args);

builder.AddDataBaseConfiguration();
builder.AddSessionConfig();
builder.Services.AddControllersWithViews();
builder.Services.AddDominioServices();


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
