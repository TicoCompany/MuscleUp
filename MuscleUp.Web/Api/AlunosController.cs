using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.Pagination;
using MuscleUp.Dominio.ViewModels.Alunos;

namespace MuscleUp.Web.Api;

public class AlunosController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;
    private readonly IAlunoService _alunoService;

    public AlunosController(IAppDbContext appDbContext, IAlunoService alunoService)
    {
        _appDbContext = appDbContext;
        _alunoService = alunoService;
    }

    [HttpPost]
    public IActionResult Salvar([FromBody] AlunoRequest request)
    {
        try
        {
            var result = _alunoService.Salvar(request);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            return Sucesso("Aluno salvo com suceso!");

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] PaginationFilter filter)
    {
        try
        {
            var result = _alunoService.Listar();
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var paginedQuery = await PaginatedList<Aluno>.CreateAsync(result.Dados!, filter.Pagina, filter.PorPagina);

            return Sucesso(new
            {
                alunos = paginedQuery.Items!.Select(q => new
                {
                    q.Usuario.Nome,
                    q.Usuario.Email,
                    Id = q.IdUsuario,
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
        var result = _alunoService.Deletar(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(result.Mensagem!);
    }

    [HttpGet, Route("{id:int}")]
    public IActionResult BuscarPorId([FromRoute] int id)
    {
        var result = _alunoService.BuscarPorId(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(new
        {
            result.Dados!.Usuario.Nome,
            result.Dados.Usuario.Email,
            result.Dados.Altura,
            result.Dados.ProblemasMedicos,
            result.Dados.Objetivo,
            result.Dados.Peso,
            Id = result.Dados.IdUsuario,
        });
    }
}
