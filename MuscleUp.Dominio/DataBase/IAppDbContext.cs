using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Academias;
using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio.DataBase;

public interface IAppDbContext
{
    DbSet<Usuario> Usuarios { get; set; }
    DbSet<Aluno> Alunos { get; set; }
    DbSet<Academia> Academias { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
