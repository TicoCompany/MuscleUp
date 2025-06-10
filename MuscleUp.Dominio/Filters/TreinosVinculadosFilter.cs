using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleUp.Dominio.Filters;

public sealed record TreinosVinculadosFilter : PaginationFilter
{
    public int IdAluno { get; set; }   
}
