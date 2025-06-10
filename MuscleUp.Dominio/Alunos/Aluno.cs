using MuscleUp.Dominio.GruposMuscularesTrabalhados;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio.Alunos;

public class Aluno
{
    public int IdUsuario {  get; set; }   
    public string? Objetivo {  get; set; }   
    public string? ProblemasMedicos {  get; set; }   
    public int? Peso {  get; set; }   
    public int? Altura {  get; set; }  

    public virtual Usuario Usuario { get; set; }
    public virtual ICollection<Treino> Treinos { get; set; } = new HashSet<Treino>();
    public virtual ICollection<TreinoPublicoEDestinadoDoAluno> TreinosPublicosEDestinados { get; set; } = new HashSet<TreinoPublicoEDestinadoDoAluno>();


}
