using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio.Treinos;

public class TreinoPublicoEDestinadoDoAluno
{
    public int Id { get; set; }
    public int IdTreino { get; set; }
    public int? IdProfessorQueDestinou { get; set; }
    public int IdAluno { get; set; }

    public virtual Treino Treino { get; set; }
    public virtual Aluno Aluno { get; set; }
    public virtual Usuario ProfessorQueDestinou { get; set; }
}
