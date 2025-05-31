namespace MuscleUp.Dominio.Filters;

public sealed record ExercicioFilter : PaginationFilter
{
    public int? IdAcademia { get; set; }
}
