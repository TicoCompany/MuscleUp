namespace MuscleUp.Dominio.ViewModels.Alunos;

public sealed record AlunoRequest
{
    public int? Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Objetivo { get; set; }
    public string? ProblemasMedicos { get; set; }
    public int? Peso { get; set; }
    public int? Altura { get; set; }
}
