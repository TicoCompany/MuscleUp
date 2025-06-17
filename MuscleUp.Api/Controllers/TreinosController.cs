using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.Pagination;
using MuscleUp.Dominio.Treinos;

namespace MuscleUp.Api.Controllers;

public class TreinosController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;

    public TreinosController(IAppDbContext appDbContext) => _appDbContext = appDbContext;

    [HttpGet, Route("ListarTreinoPorAluno/{id:int}")]
    public async Task<IActionResult> List([FromQuery] TreinoFilter filter, [FromRoute] int id)
    {
        try
        {
            var treinos = _appDbContext.Treinos.AsNoTracking().Include(q => q.GruposMuscularesTrabalhados).ThenInclude(q => q.ExerciciosDoTreino).Where(q => q.IdAluno == UsuarioLogado.Id).ToList();

            var treinosDestinadosDoAluno = _appDbContext.TreinosPublicosEDestinadosDoAluno.Include(q => q.Treino).Where(q => q.IdAluno == UsuarioLogado.Id).Select(q => q.Treino).ToList();


            return Sucesso(new
            {
            });

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }
}
