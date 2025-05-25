namespace MuscleUp.Dominio.Filters;

public sealed record ProfessorFilter : PaginationFilter
{
    public int IdAcademia { get; set; }
}
