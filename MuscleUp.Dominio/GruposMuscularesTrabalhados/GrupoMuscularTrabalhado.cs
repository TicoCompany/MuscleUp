using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.Exercicios.Enums;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Dominio.GruposMuscularesTrabalhados;

public class GrupoMuscularTrabalhado
{
    public int Id { get; set; }
    public int IdTreino { get; set; }
    public GrupoMuscular GrupoMuscular { get; set; }
    public DivisaoDeSubTreino DivisaoDeTreino { get; set; }

    public virtual Treino Treino { get; set; }

    public virtual ICollection<ExercicioDoTreino> ExerciciosDoTreino { get; set; } = new HashSet<ExercicioDoTreino>();
}
