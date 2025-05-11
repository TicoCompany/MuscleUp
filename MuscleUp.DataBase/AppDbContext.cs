using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.DataBase;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }

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