using Microsoft.Extensions.DependencyInjection;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio;

public static class DependencyInjectionConfig
{
    public static void AddDominioServices(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioService, UsuarioService>();
    }
}
