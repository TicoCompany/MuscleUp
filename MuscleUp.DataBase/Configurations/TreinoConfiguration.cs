using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleUp.Dominio.Treinos;

namespace MuscleUp.DataBase.Configurations;

internal class TreinoConfiguration : IEntityTypeConfiguration<Treino>
{
    public void Configure(EntityTypeBuilder<Treino> builder)
    {
        builder.ToTable("treinos");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).HasMaxLength(128);
        builder.Property(c => c.Tempo).HasMaxLength(12);


        builder.HasOne(q => q.Academia)
               .WithMany(q => q.Treinos)
               .HasForeignKey(q => q.IdAcademia)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.Aluno)
               .WithMany(q => q.Treinos)
               .HasForeignKey(q => q.IdAluno)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(q => q.Professor)
               .WithMany(q => q.TreinosCriados)
               .HasForeignKey(q => q.IdProfessor)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(q => q.GruposMuscularesTrabalhados)
               .WithOne(q => q.Treino)
               .HasForeignKey(q => q.IdTreino)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.TreinosPublicosEDestinasdoDoAluno)
               .WithOne(q => q.Treino)
               .HasForeignKey(q => q.IdTreino)
               .OnDelete(DeleteBehavior.Cascade);

    }

}
