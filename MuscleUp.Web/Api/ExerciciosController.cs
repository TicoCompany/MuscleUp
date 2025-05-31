using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.Pagination;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.ViewModels.Exercicios;

namespace MuscleUp.Web.Api;

public class ExerciciosController : BaseApiController
{
    private readonly IExercicioService _exercicioService;

    public ExerciciosController(IExercicioService exercicio)
    {
        _exercicioService = exercicio;
    }

    [HttpPost]
    public IActionResult Salvar([FromBody] ExercicioRequest request)
    {
        try
        {
            if (!request.IdAcademia.HasValue)
                request.IdAcademia = UsuarioLogado.Id;

            var result = _exercicioService.Salvar(request);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);


            return Sucesso(result.Mensagem!);

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ExercicioFilter filter)
    {
        try
        {
            var result = _exercicioService.Listar(filter);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var paginedQuery = await PaginatedList<Exercicio>.CreateAsync(result.Dados!, filter.Pagina, filter.PorPagina);

            return Sucesso(new
            {
                Exercicios = paginedQuery.Items!.Select(q => new
                {
                    q.Nome,
                    q.Id,
                    nomeDaAcademia = q.Academia!.Nome,
                }),
                TotalPaginas = paginedQuery.TotalPages
            });

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpDelete, Route("{id:int}")]
    public IActionResult Excluir([FromRoute] int id)
    {
        var result = _exercicioService.Deletar(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(result.Mensagem!);
    }

    [HttpGet, Route("{id:int}")]
    public IActionResult BuscarPorId([FromRoute] int id)
    {
        var result = _exercicioService.BuscarPorId(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(new
        {
            result.Dados!.Nome,
            result.Dados!.Id,
        });
    }
}
