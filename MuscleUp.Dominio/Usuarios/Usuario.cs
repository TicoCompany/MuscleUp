using System.ComponentModel.DataAnnotations.Schema;
using MuscleUp.Dominio.Academias;
using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.Treinos;

namespace MuscleUp.Dominio.Usuarios;

[Table("usuarios")]
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

    public virtual ICollection<TreinoPublicoEDestinadoDoAluno> TreinosMinistrados { get; set; } = new HashSet<TreinoPublicoEDestinadoDoAluno>();
    public virtual ICollection<Treino> TreinosCriados { get; set; } = new HashSet<Treino>();

}
