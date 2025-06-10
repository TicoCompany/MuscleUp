using MuscleUp.Dominio.Exercicios.Enums;
using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Dominio.ViewModels.Treinos;

public sealed record TreinoResponse
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string NomeDaDivisao { get; set; }
    public string DificuldadeDoTreino { get; set; }
    public bool Publico { get; set; }
    public string Tempo { get; set; }
    public List<DivisoesDoTreinoResponse> Divisoes { get; set; } = new List<DivisoesDoTreinoResponse>();
}

public sealed record DivisoesDoTreinoResponse
{
    public string NomeDaDivisaoDoSubTreino { get; set; }
    public List<MembroMusculareResponse> Membros { get; set; } = new List<MembroMusculareResponse>();
}

public sealed record MembroMusculareResponse
{
    public int Id { get; set; }
    public string NomeDoGrupoMuscular { get; set; }
    public GrupoMuscular GrupoMuscular { get; set; }
    public List<ExercicioDoTreinoResponse> Exercicios { get; set; } = new List<ExercicioDoTreinoResponse>();
}

public sealed record ExercicioDoTreinoResponse
{
    public int Id { get; set; }
    public int IdExercicio { get; set; }
    public int Serie { get; set; }
    public string Nome { get; set; }
    public int Repeticao { get; set; }
    public string ImagemDoExercicio { get; set; }
}