using MuscleUp.Dominio.Academias;
using MuscleUp.Dominio.Alunos;

namespace MuscleUp.Dominio.Usuarios;

public class Usuario
{
    public int Id { get; set; }
    public int? IdAcademia { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public string? Telefone { get; set; }

    public virtual Aluno? Aluno { get; set; }    
    public virtual Academia? Academia { get; set; }    
}
