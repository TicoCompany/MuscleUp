namespace MuscleUp.Dominio.ViewModels.Treinos;

public sealed record EnviarTreinoParaAluno
{
    public int IdTreino { get; set; }
    public List<int> IdsDosALunos { get; set; } = new List<int>();
}
