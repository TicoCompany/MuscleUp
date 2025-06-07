using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Academias;
using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio.DataBase;

public interface IAppDbContext
{
    DbSet<Usuario> Usuarios { get; set; }
    DbSet<Aluno> Alunos { get; set; }
    DbSet<Academia> Academias { get; set; }
    DbSet<Exercicio> Exercicios { get; set; }
    DbSet<Treino> Treinos { get; set; }
    DbSet<GrupoMuscularTrabalhado> GruposMuscularesTrabalhados { get; set; }



    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
