using Microsoft.Extensions.DependencyInjection;
using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.Contas;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.Mensageria;
using MuscleUp.Dominio.Professores;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio;

public static class DependencyInjectionConfig
{
    public static void AddDominioServices(this IServiceCollection services)
    {
        services.Servicos();
        services.Mensageria();
    }

    public static void Servicos(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IAlunoService, AlunoService>();
        services.AddScoped<IContaService, ContaService>();
        services.AddScoped<IProfessorService, ProfessorService>();
        services.AddScoped<IExercicioService, ExercicioService>();

    }

    public static void Mensageria(this IServiceCollection services)
    {
        services.AddScoped<IEnviadorDeEmail, EnviadorDeEmail>();
        services.AddScoped<IEmailService, EmailService>();
    }
}
