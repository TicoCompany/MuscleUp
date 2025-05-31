using System.ComponentModel.DataAnnotations;

namespace MuscleUp.Dominio.Exercicios.Enums;

public enum DificuldadeDoExercicio
{
    [Display(Name = "Fácil")]
    Facil,
    [Display(Name = "Médio")]
    Medio,
    [Display(Name = "Difícil")]
    Dificil
}
