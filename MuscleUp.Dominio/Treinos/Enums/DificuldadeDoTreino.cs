using System.ComponentModel.DataAnnotations;

namespace MuscleUp.Dominio.Treinos.Enums;

public enum DificuldadeDoTreino
{
    Iniciante = 0,
    Intermediario = 1,
    [Display(Name = "Avançado")]
    Avancado = 2,
    Intenso = 3,
}
