using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleUp.Dominio.Treinos;

namespace MuscleUp.DataBase.Configurations;

internal class TreinoPublicoEDestinadoDoAlunoConfiguration : IEntityTypeConfiguration<TreinoPublicoEDestinadoDoAluno>
{
    public void Configure(EntityTypeBuilder<TreinoPublicoEDestinadoDoAluno> builder)
    {
        builder.ToTable("treinospublicosedestinadosdoaluno");

        builder.HasKey(c => c.Id);

        builder.HasOne(q => q.Treino)
               .WithMany(q => q.TreinosPublicosEDestinasdoDoAluno)
               .HasForeignKey(q => q.IdTreino)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.Aluno)
               .WithMany(q => q.TreinosPublicosEDestinados)
               .HasForeignKey(q => q.IdAluno)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.ProfessorQueDestinou)
               .WithMany(q => q.TreinosMinistrados)
               .HasForeignKey(q => q.IdProfessorQueDestinou)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
