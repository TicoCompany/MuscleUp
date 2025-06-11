using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;

namespace MuscleUp.DataBase.Configurations;

internal class GrupoMuscularTrabalhadoConfiguration : IEntityTypeConfiguration<GrupoMuscularTrabalhado>
{
    public void Configure(EntityTypeBuilder<GrupoMuscularTrabalhado> builder)
    {
        builder.ToTable("gruposmuscularestrabalhados");
        builder.HasKey(c => c.Id);

        builder.HasOne(q => q.Treino).WithMany(q => q.GruposMuscularesTrabalhados).HasForeignKey(q => q.IdTreino);
    }
}
