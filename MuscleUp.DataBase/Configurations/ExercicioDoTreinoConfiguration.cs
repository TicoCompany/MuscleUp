using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleUp.Dominio.Exercicios;

namespace MuscleUp.DataBase.Configurations;

internal class ExercicioDoTreinoConfiguration : IEntityTypeConfiguration<ExercicioDoTreino>
{
    public void Configure(EntityTypeBuilder<ExercicioDoTreino> builder)
    {
        builder.ToTable("ExerciciosDoTreino");
        builder.HasKey(q => q.Id);

        builder.HasOne(q => q.Exercicio).WithMany(q => q.ExerciciosDosTreinosVinculados).HasForeignKey(q => q.IdExercicio);
    }
}
