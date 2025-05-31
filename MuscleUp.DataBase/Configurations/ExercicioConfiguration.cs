using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleUp.Dominio.Exercicios;


namespace MuscleUp.DataBase.Configurations;

internal class ExercicioConfiguration : IEntityTypeConfiguration<Exercicio>
{
    public void Configure(EntityTypeBuilder<Exercicio> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).HasMaxLength(64);
        builder.Property(c => c.Descricao).HasMaxLength(128);

        builder.HasOne(q => q.Academia).WithMany(q => q.Exercicios).HasForeignKey(q => q.IdAcademia).IsRequired(false).OnDelete(DeleteBehavior.Cascade);

    }

}
