using Microsoft.EntityFrameworkCore;
using MuscleUp.DataBase;
using MuscleUp.Dominio.Auth;
using MuscleUp.Dominio.DataBase;

namespace MuscleUp.Web.Configurations;

public static class SessionConfiguration
{
    public static void AddSessionConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Cookies")
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/api/Contas/login";
                options.LogoutPath = "/api/Contas/logout";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

            });

        builder.Services.AddAuthorization();
    }
}
