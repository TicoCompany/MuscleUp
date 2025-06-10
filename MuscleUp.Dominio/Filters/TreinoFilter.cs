using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuscleUp.Dominio.Exercicios.Enums;
using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Dominio.Filters;

public sealed record TreinoFilter : PaginationFilter
{
    public int? IdAcademia { get; set; }
    public DificuldadeDoTreino DificuldadeDoTreino { get; set; }
    public DificuldadeDoTreino DivisaoDeTreino { get; set; }
}
