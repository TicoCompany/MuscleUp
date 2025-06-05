namespace MuscleUp.Dominio.Filters;

public sealed record AlunoFilter : PaginationFilter
{
    public int? IdAcademia { get; set; }
}
