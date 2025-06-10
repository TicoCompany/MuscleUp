namespace MuscleUp.Dominio.ViewModels.Treinos;

public sealed record Step2Response
{
    public int GrupoMuscular { get; set; }
    public int DivisaoDeSubTreino { get; set; }
    public int IdDoMembro { get; set; }
}
