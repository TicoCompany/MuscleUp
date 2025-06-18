using MuscleUp.Dominio.Exercicios.Enums;
using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Dominio.ViewModels.Treinos.Mobile;

public sealed record TreinoMobileRequest
{
    public int? Id { get; init; }
    public required string Name { get; init; }
    public DivisaoDeTreino Type { get; init; }
    public List<MuscleDayRequest> MuscleDays { get; init; } = new();
}

public sealed record MuscleDayRequest
{
    public int Id { get; init; }
    public DivisaoDeSubTreino Type { get; init; }
    public GrupoMuscular MuscleGroup { get; init; } 
    public List<ExerciseRequest> Exercises { get; init; } = new();
}

public sealed record ExerciseRequest
{
    public int Id { get; init; }
    public int IdExercicio { get; init; }
    public int Sets { get; init; }
    public int Reps { get; init; }
}
