using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.DataBase.Configurations;

internal class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).HasMaxLength(100);
    }
}