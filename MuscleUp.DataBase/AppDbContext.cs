using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Academias;
using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.DataBase;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Academia> Academias { get; set; }
    public DbSet<Aluno> Alunos { get; set; }
    public DbSet<Exercicio> Exercicios { get; set; }
    public DbSet<Treino> Treinos { get; set; }
    public DbSet<GrupoMuscularTrabalhado> GruposMuscularesTrabalhados { get; set; }
    public DbSet<ExercicioDoTreino> ExerciciosDoTreino { get; set; }

    public int SaveChanges()
    {
        return base.SaveChanges();
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}