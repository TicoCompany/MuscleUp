using MuscleUp.Dominio.Academias;
using MuscleUp.Dominio.Exercicios.Enums;

namespace MuscleUp.Dominio.Exercicios;

public class Exercicio
{
    public int Id { get; set; }
    public int? IdAcademia { get; set; }
    public string Nome { get; set; }
    public string Caminho { get; set; }
    public GrupoMuscular GrupoMuscular { get; set; }
    public string PublicId { get; set; } //PARA IMAGEM DO CLOUDINARY
    public string Descricao { get; set; }
    public DificuldadeDoExercicio Dificuldade { get; set; }

    public virtual Academia Academia { get; set; }
}
