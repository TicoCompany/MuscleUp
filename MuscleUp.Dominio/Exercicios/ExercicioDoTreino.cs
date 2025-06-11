using System.ComponentModel.DataAnnotations.Schema;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;

namespace MuscleUp.Dominio.Exercicios;
[Table("exerciciosdotreino")]
public class ExercicioDoTreino
{
    public int Id { get; set; }
    public int IdExercicio { get; set; }
    public int IdMembroTrabalhado { get; set; }
    public int Repeticao { get; set; }
    public int Serie { get; set; }

    public virtual Exercicio Exercicio { get; set; }
    public virtual GrupoMuscularTrabalhado GrupoMuscularTrabalhado { get; set; }

}
