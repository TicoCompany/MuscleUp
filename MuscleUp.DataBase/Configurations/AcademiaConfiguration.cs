using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Academias;

namespace MuscleUp.DataBase.Configurations;

internal class AcademiaConfiguration : IEntityTypeConfiguration<Academia>
{
    public void Configure(EntityTypeBuilder<Academia> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome)
               .IsRequired()
               .HasMaxLength(128);

        builder.Property(a => a.Cnpj)
               .HasMaxLength(18);

        builder.Property(a => a.Cep)
               .HasMaxLength(9);

        builder.Property(a => a.Logradouro)
               .HasMaxLength(128);

        builder.Property(a => a.Bairro)
               .HasMaxLength(64);

        builder.Property(a => a.Estado)
               .HasMaxLength(2);

        builder.Property(a => a.Cidade)
               .HasMaxLength(64);

        builder.Property(a => a.Numero)
               .HasMaxLength(16);

        builder.HasMany(a => a.Usuarios)
               .WithOne(u => u.Academia)
               .HasForeignKey(u => u.IdAcademia)
               .OnDelete(DeleteBehavior.SetNull);
    }
}