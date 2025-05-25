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
    
}
