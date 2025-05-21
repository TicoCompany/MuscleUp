using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleUp.Dominio.Alunos;

namespace MuscleUp.DataBase.Configurations;

internal class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
{
    public void Configure(EntityTypeBuilder<Aluno> builder)
    {
        builder.HasKey(c => c.IdUsuario);
        builder.Property(c => c.Objetivo).HasMaxLength(512);
        builder.Property(c => c.ProblemasMedicos).HasMaxLength(512);

        builder.HasOne(q => q.Usuario).WithOne(q => q.Aluno).HasForeignKey<Aluno>(q => q.IdUsuario);
    }
}
