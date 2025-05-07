using Microsoft.EntityFrameworkCore;
using MuscleUp.DataBase;
using MuscleUp.Dominio.DataBase;

namespace MuscleUp.Web.Configurations;

public static class DataBaseConfiguration
{
    public static void AddDataBaseConfiguration(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        builder.Services.AddTransient<IAppDbContext, AppDbContext>();
    }
}
