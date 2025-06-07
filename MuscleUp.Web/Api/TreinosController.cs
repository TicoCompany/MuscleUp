using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.Treinos.Enums;
using MuscleUp.Dominio.ViewModels.Treinos;
using MuscleUp.Dominio.ViewModels.Usuarios;

namespace MuscleUp.Web.Api;

public class TreinosController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;

    public TreinosController(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
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

            foreach (var divisao in request.Divisoes)
            {
                foreach (var membros in divisao.Membros)
                {

                    var membroTrabalhado = new GrupoMuscularTrabalhado
                    {
                        Id = membros.Id ?? 0,
                        GrupoMuscular = membros.GrupoMuscular,
                        IdTreino = treino.Id,
                        DivisaoDeTreino = divisao.DivisaoDeSubTreino,
                    };

                    if (membros.Id.HasValue)
                        _appDbContext.GruposMuscularesTrabalhados.Update(membroTrabalhado);
                    else
                        _appDbContext.GruposMuscularesTrabalhados.Add(membroTrabalhado);
                }

            }
            _appDbContext.SaveChanges();

            return Sucesso("");
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
