using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Dominio.Filters;

public sealed record TreinoFilter : PaginationFilter
{
    public int? IdAcademia { get; set; }
    public DificuldadeDoTreino DificuldadeDoTreino { get; set; }
    public DificuldadeDoTreino DivisaoDeTreino { get; set; }
}
