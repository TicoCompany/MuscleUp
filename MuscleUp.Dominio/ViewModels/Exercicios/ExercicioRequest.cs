using MuscleUp.Dominio.Exercicios.Enums;

namespace MuscleUp.Dominio.ViewModels.Exercicios;

public sealed record ExercicioRequest
{
    public int Id { get; set; }
    public int IdAcademia { get; set; }
    public string? PublicId { get; set; }
    public string Nome { get; set; }
    public string? Caminho { get; set; }
    public string Descricao { get; set; }
    public GrupoMuscular GrupoMuscular { get; set; }
    public DificuldadeDoExercicio Dificuldade { get; set; }
}
