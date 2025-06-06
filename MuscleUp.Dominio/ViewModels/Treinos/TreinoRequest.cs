using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Dominio.ViewModels.Treinos;

public sealed record TreinoRequest
{
    public int? Id { get; set; }
    public string Nome { get; set; }
    public DivisaoDeTreino Divisao { get; set; }
    public bool Publico { get; set; }
    public string Tempo { get; set; }
}
