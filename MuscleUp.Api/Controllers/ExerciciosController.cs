using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;

namespace MuscleUp.Api.Controllers;

public class ExerciciosController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;

    public ExerciciosController(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet]
    public IActionResult List([FromQuery] ExercicioFilter filter)
    {
        try
        {
            var exercicios = _appDbContext.Exercicios.AsNoTracking()
                .Where(q => (q.IdAcademia == null || q.IdAcademia == UsuarioLogado.IdAcademia) && q.GrupoMuscular == filter.GrupoMuscular).ToList();

            return Sucesso(new
            {
                Exercicios = exercicios
            });

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }
}
