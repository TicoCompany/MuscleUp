using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio.DataBase;

public interface IAppDbContext
{
    DbSet<Usuario> Usuarios { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
