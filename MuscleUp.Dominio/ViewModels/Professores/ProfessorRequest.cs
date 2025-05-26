namespace MuscleUp.Dominio.ViewModels.Professores;

public class ProfessorRequest
{
    public int? Id { get; set; }
    public int? IdAcademia { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Senha { get; set; }
    public DateTime? DataDeNascimento { get; set; }
}
