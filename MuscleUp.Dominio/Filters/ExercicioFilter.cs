using MuscleUp.Dominio.Exercicios.Enums;

namespace MuscleUp.Dominio.Filters;

public sealed record ExercicioFilter : PaginationFilter
{
    public DificuldadeDoExercicio? Dificuldade { get; set; }
    public GrupoMuscular? GrupoMuscular { get; set; }
}
