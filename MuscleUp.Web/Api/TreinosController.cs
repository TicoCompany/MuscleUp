using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Alunos;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;
using MuscleUp.Dominio.Pagination;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.Treinos.Enums;
using MuscleUp.Dominio.Usuarios;
using MuscleUp.Dominio.ViewModels.Treinos;

namespace MuscleUp.Web.Api;

public class TreinosController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;
    private readonly ITreinoService _treinoService;

    public TreinosController(IAppDbContext appDbContext, ITreinoService treinoService)
    {
        _appDbContext = appDbContext;
        _treinoService = treinoService;
    }

    [HttpPost, Route("Step1")]
    public IActionResult SalvarInformacoesDoTreino([FromBody] TreinoRequest request)
    {
        try
        {
            var result = _treinoService.SalvarStep1(request, UsuarioLogado);

            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var dados = result.Dados!;

            return Sucesso(new { Id = dados.First().IdTreino, divisoes = dados });
        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpPost, Route("Step2")]
    public IActionResult SalvarGruposMusculares([FromBody] TreinoRequest request)
    {
        try
        {
            var result = _treinoService.SalvarStep2(request);

            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var dados = result.Dados!;

            return Sucesso(new { MembrosSalvos = dados });
        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpPost, Route("Step3")]
    public IActionResult SalvarExercicios([FromBody] TreinoRequest request)
    {
        try
        {
            var result = _treinoService.SalvarStep3(request);

            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var dados = result.Dados!;

            return Sucesso(result.Mensagem!);
        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpPost, Route("EnviarParaAlunos")]
    public IActionResult EnviarParaAlunos([FromBody] EnviarTreinoParaAluno request)
    {
        try
        {
            var result = _treinoService.EnviarParaAlunos(request, UsuarioLogado.Id);

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
    public async Task<IActionResult> List([FromQuery] TreinoFilter filter)
    {
        try
        {
            var result = _treinoService.Listar(filter);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var paginedQuery = await PaginatedList<Treino>.CreateAsync(result.Dados!, filter.Pagina, filter.PorPagina);

            return Sucesso(new
            {
                Treinos = paginedQuery.Items!.Select(q => new
                {
                    q.Nome,
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

    [HttpGet, Route("{id:int}")]
    public IActionResult BuscarPorId([FromRoute] int id)
    {
        var result = _treinoService.BuscarPorId(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        var dados = result.Dados!;

        var treino = new
        {
            dados.Id,
            dados.DificuldadeDoTreino,
            dados.Publico,
            dados.Divisao,
            dados.Tempo,
            dados.Nome,
            Divisoes = dados.GruposMuscularesTrabalhados.GroupBy(q => q.DivisaoDeTreino).Select(g => new
            {
                DivisaoDeSubTreino = g.Key,
                NomeDaDivisao = ((DivisaoDeSubTreino)g.Key).DisplayName(),
                Membros = g.Select(v => new
                {
                    v.Id,
                    v.GrupoMuscular,
                    Nome = v.GrupoMuscular.DisplayName(),
                    Exercicios = v.ExerciciosDoTreino.Select(e => new
                    {
                        e.Id,
                        e.IdExercicio,
                        e.Exercicio.Nome,
                        e.Serie,
                        e.Repeticao,
                        e.Exercicio.Caminho,
                    }).ToList(),
                }).ToList()
            }).ToList()
        };

        return Sucesso(new { Treino = treino });
    }

    [HttpDelete, Route("{id:int}")]
    public IActionResult Excluir([FromRoute] int id)
    {
        var result = _treinoService.Deletar(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(result.Mensagem!);
    }

    [HttpDelete, Route("ExcluirGrupoMuscular/{id:int}")]
    public IActionResult ExcluirGrupoMuscular([FromRoute] int id)    
    {
        var result = _treinoService.DeletarGrupoMuscular(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(result.Mensagem!);
    }


}
