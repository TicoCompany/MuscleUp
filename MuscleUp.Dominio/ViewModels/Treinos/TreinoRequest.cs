using MuscleUp.Dominio.Exercicios.Enums;
using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Dominio.ViewModels.Treinos;

public sealed record TreinoRequest
{
    public int? Id { get; set; }
    public string Nome { get; set; }
    public DivisaoDeTreino Divisao { get; set; }
    public DificuldadeDoTreino DificuldadeDoTreino { get; set; }
    public bool Publico { get; set; }
    public string Tempo { get; set; }
    public List<DivisoesDoTreinoRequest> Divisoes { get; set; } = new List<DivisoesDoTreinoRequest>();
}

public sealed record DivisoesDoTreinoRequest
{
    public DivisaoDeSubTreino DivisaoDeSubTreino { get; set; }
    public List<MembroMusculareRequest> Membros { get; set; } = new List<MembroMusculareRequest>();
}

public sealed record MembroMusculareRequest
{
    public int? Id { get; set; }
    public GrupoMuscular GrupoMuscular { get; set; }
    public List<ExercicioDoTreinoRequest> Exercicios { get; set; } = new List<ExercicioDoTreinoRequest>();
}

public sealed record ExercicioDoTreinoRequest
{
    public int? Id { get; set; }
    public int IdExercicio { get; set; }
    public int Serie { get; set; }
    public int Repeticao { get; set; }
    public string Caminho { get; set; }
}