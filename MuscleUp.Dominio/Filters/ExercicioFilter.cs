using MuscleUp.Dominio.Exercicios.Enums;

namespace MuscleUp.Dominio.Filters;

public sealed record ExercicioFilter : PaginationFilter
{
    public int? IdAcademia { get; set; }
    public DificuldadeDoExercicio? Dificuldade { get; set; }
    public GrupoMuscular? GrupoMuscular { get; set; }
}
