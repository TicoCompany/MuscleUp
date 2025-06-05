using System.ComponentModel.DataAnnotations;

namespace MuscleUp.Dominio.Exercicios.Enums;

public enum GrupoMuscular
{
    Peito,
    Costas,
    Ombros,
    
    [Display(Name = "Bíceps")]
    Biceps,

    [Display(Name = "Tríceps")]
    Triceps,

    [Display(Name = "Antebraço")]
    Antebraco,

    [Display(Name = "Abdômen")]
    Abdomen,

    [Display(Name = "Quadríceps")]
    Quadriceps,

    [Display(Name = "Posterior de coxa")]
    PosteriorCoxa,

    [Display(Name = "Glúteos")]
    Gluteos,

    Panturrilhas = 11
}
