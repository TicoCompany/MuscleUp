using MuscleUp.Dominio.Academias;
using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;
using MuscleUp.Dominio.Treinos.Enums;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio.Treinos;

public class Treino
{
    public int Id { get; set; }
    public int? IdAluno { get; set; }
    public int IdAcademia { get; set; }
    public int? IdProfessor { get; set; }
    public string Nome { get; set; }
    public DivisaoDeTreino Divisao { get; set; }
    public bool Publico { get; set; }
    public string Tempo { get; set; }

    public virtual Aluno? Aluno { get; set; }
    public virtual Usuario? Professor { get; set; }
    public virtual Academia Academia { get; set; }

    public virtual ICollection<GrupoMuscularTrabalhado> GruposMuscularesTrabalhados { get; set; } = new HashSet<GrupoMuscularTrabalhado>();
    public virtual ICollection<TreinoPublicoEDestinadoDoAluno> TreinosPublicosEDestinasdoDoAluno { get; set; } = new HashSet<TreinoPublicoEDestinadoDoAluno>();
}
