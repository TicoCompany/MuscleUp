using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.Pagination;
using MuscleUp.Dominio.Professores;
using MuscleUp.Dominio.Usuarios;
using MuscleUp.Dominio.ViewModels.Professores;

namespace MuscleUp.Web.Api;

public class ProfessoresController : BaseApiController
{

    private readonly IAppDbContext _appDbContext;
    private readonly IProfessorService _professorService;

    public ProfessoresController(IAppDbContext appDbContext, IProfessorService professorService)
    {
        _appDbContext = appDbContext;
        _professorService = professorService;
    }

    [HttpPost]
    public IActionResult Salvar([FromBody] ProfessorRequest request)
    {
        try
        {
            var result = _professorService.Salvar(request);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            return Sucesso("Professor salvo com suceso!");

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ProfessorFilter filter)
    {
        try
        {
            var result = _professorService.Listar(filter);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var paginedQuery = await PaginatedList<Usuario>.CreateAsync(result.Dados!, filter.Pagina, filter.PorPagina);

            return Sucesso(new
            {
                Professors = paginedQuery.Items!.Select(q => new
                {
                    q.Nome,
                    q.Email,
                    q.Id,
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
        var result = _professorService.Deletar(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(result.Mensagem!);
    }

    [HttpGet, Route("{id:int}")]
    public IActionResult BuscarPorId([FromRoute] int id)
    {
        var result = _professorService.BuscarPorId(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(new
        {
            result.Dados!.Email,
            result.Dados!.Nome,
            result.Dados!.Id,
        });
    }
}
