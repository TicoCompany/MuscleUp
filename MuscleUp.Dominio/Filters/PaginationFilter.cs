namespace MuscleUp.Dominio.Filters;
public sealed record PaginationFilter
{
    public string? Busca { get; set; }
    public int Page { get; set; }
    public int QuantidadeDeItens { get; set; }
}
