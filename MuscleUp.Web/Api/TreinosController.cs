using Microsoft.AspNetCore.Mvc;
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
            var treino = new Treino
            {
                Id = request.Id ?? 0,
                IdAcademia = UsuarioLogado.IdAcademia ?? 1,

                Nome = request.Nome,
                Divisao = request.Divisao,
                Publico = request.Publico,
                Tempo = request.Tempo,
            };

            if (!request.Id.HasValue)
                _appDbContext.Treinos.Add(treino);
            else
                _appDbContext.Treinos.Update(treino);

            _appDbContext.SaveChanges();

            var divisoesDeSubTreino = ObterDivisoes(treino.Divisao);
            var divisoes = divisoesDeSubTreino.Select(q => new
            {
                DivisaoDeSubTreino = (int)q,
                NomeDaDivisao = q.DisplayName()
            });

            return Sucesso(new { Id = treino.Id, divisoes = divisoes });
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
            var treino = _appDbContext.Treinos.FirstOrDefault(q => q.Id == request.Id);
            if (treino == null)
                return Erro("Treino não encontrado!");

            List<GrupoMuscularTrabalhado> membrosMusculares = new List<GrupoMuscularTrabalhado>();
            foreach (var divisao in request.Divisoes)
            {
                foreach (var membro in divisao.Membros)
                {
                    var membroTrabalhado = new GrupoMuscularTrabalhado
                    {
                        Id = membro.Id ?? 0,
                        GrupoMuscular = membro.GrupoMuscular,
                        IdTreino = treino.Id,
                        DivisaoDeTreino = divisao.DivisaoDeSubTreino,
                    };

                    if (membro.Id.HasValue)
                        _appDbContext.GruposMuscularesTrabalhados.Update(membroTrabalhado);
                    else
                        _appDbContext.GruposMuscularesTrabalhados.Add(membroTrabalhado);

                    membrosMusculares.Add(membroTrabalhado);
                }

            }
            _appDbContext.SaveChanges();

            var membrosSalvos = membrosMusculares.Select(q => new
            {
                GrupoMuscular = q.GrupoMuscular,
                DivisaoDeSubTreino = q.DivisaoDeTreino,
                IdDoMembro = q.Id,
            });

            return Sucesso(new { MembrosSalvos = membrosSalvos });
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
            var treino = _appDbContext.Treinos.FirstOrDefault(q => q.Id == request.Id);
            if (treino == null)
                return Erro("Treino não encontrado!");

            foreach (var divisao in request.Divisoes)
            {
                foreach (var membro in divisao.Membros)
                {
                    foreach (var q in membro.Exercicios)
                    {
                        var exercicio = new ExercicioDoTreino
                        {
                            Id = q.Id ?? 0,
                            IdExercicio = q.IdExercicio,
                            IdMembroTrabalhado = membro.Id ?? throw new Exception("Um erro inesperado aconteceu"),
                            Repeticao = q.Repeticao,
                            Serie = q.Serie
                        };

                        if (exercicio.Id == 0)
                            _appDbContext.ExerciciosDoTreino.Add(exercicio);
                        else
                            _appDbContext.ExerciciosDoTreino.Update(exercicio);
                    }
                }

            }
            _appDbContext.SaveChanges();

            return Sucesso("Salvou!");
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

    private List<DivisaoDeSubTreino> ObterDivisoes(DivisaoDeTreino divisao)
    {
        switch (divisao)
        {
            case DivisaoDeTreino.A:
                return new List<DivisaoDeSubTreino>
            {
                DivisaoDeSubTreino.A,
            };
            case DivisaoDeTreino.AB:
                return new List<DivisaoDeSubTreino>
            {
                DivisaoDeSubTreino.A,
                DivisaoDeSubTreino.B,
            };
            case DivisaoDeTreino.ABC:
                return new List<DivisaoDeSubTreino>
            {
                DivisaoDeSubTreino.A,
                DivisaoDeSubTreino.B,
                DivisaoDeSubTreino.C,
            };
            case DivisaoDeTreino.ABCD:
                return new List<DivisaoDeSubTreino>
            {
                DivisaoDeSubTreino.A,
                DivisaoDeSubTreino.B,
                DivisaoDeSubTreino.C,
                DivisaoDeSubTreino.D,
            };
            case DivisaoDeTreino.ABCDE:
                return new List<DivisaoDeSubTreino>
            {
                DivisaoDeSubTreino.A,
                DivisaoDeSubTreino.B,
                DivisaoDeSubTreino.C,
                DivisaoDeSubTreino.D,
                DivisaoDeSubTreino.E,
            };
            case DivisaoDeTreino.ABCDEF:
                return new List<DivisaoDeSubTreino>
            {
                DivisaoDeSubTreino.A,
                DivisaoDeSubTreino.B,
                DivisaoDeSubTreino.C,
                DivisaoDeSubTreino.D,
                DivisaoDeSubTreino.E,
                DivisaoDeSubTreino.F,
            };
            case DivisaoDeTreino.ABCDEFG:
                return new List<DivisaoDeSubTreino>
            {
                DivisaoDeSubTreino.A,
                DivisaoDeSubTreino.B,
                DivisaoDeSubTreino.C,
                DivisaoDeSubTreino.D,
                DivisaoDeSubTreino.E,
                DivisaoDeSubTreino.F,
                DivisaoDeSubTreino.G,
            };
        }

        return new List<DivisaoDeSubTreino>
            {
                DivisaoDeSubTreino.A,
            };

    }
}
