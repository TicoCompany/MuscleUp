namespace MuscleUp.Dominio.Filters;
public record PaginationFilter
{
    public string? Busca { get; set; }
    public int Pagina { get; set; }
    public int PorPagina { get; set; }
}
