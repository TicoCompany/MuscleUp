using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.Treinos.Enums;
using MuscleUp.Dominio.ViewModels.Treinos.Mobile;

namespace MuscleUp.Api.Controllers;

public class TreinosController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;

    public TreinosController(IAppDbContext appDbContext) => _appDbContext = appDbContext;

    [HttpGet, Route("ListarTreinoPorAluno/{id:int}")]
    public IActionResult List([FromQuery] TreinoFilter filter, [FromRoute] int id)
    {
        try
        {
            var treinos = _appDbContext.Treinos.AsNoTracking().Include(q => q.GruposMuscularesTrabalhados).ThenInclude(q => q.ExerciciosDoTreino).Where(q => q.IdAluno == UsuarioLogado.Id).ToList();

            var treinosDestinadosDoAluno = _appDbContext.TreinosPublicosEDestinadosDoAluno.Include(q => q.Treino).Where(q => q.IdAluno == UsuarioLogado.Id).Select(q => q.Treino).ToList();

            var todosTreinos = treinos.Concat(treinosDestinadosDoAluno).ToList();

            return Sucesso(new
            {
                Treinos = todosTreinos
            });

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpPost]
    public IActionResult Salvar(TreinoMobileRequest request)
    {
        using (TransactionScope scope = new TransactionScope())
        {
            var treino = new Treino
            {
                Id = request.Id ?? 0,
                IdAcademia = UsuarioLogado.IdAcademia ?? 1,
                Nome = request.Name,
                Divisao = request.Type,
                Publico = false,
                Tempo = "1h",
                DificuldadeDoTreino = DificuldadeDoTreino.Iniciante,
            };
            _appDbContext.Treinos.Add(treino);
            _appDbContext.SaveChanges();

            foreach (var membro in request.MuscleDays)
            {
                var membroTrabalhado = new GrupoMuscularTrabalhado
                {
                    GrupoMuscular = membro.MuscleGroup,
                    IdTreino = treino.Id,
                    DivisaoDeTreino = membro.Type,
                };

                _appDbContext.GruposMuscularesTrabalhados.Add(membroTrabalhado);
            }
            _appDbContext.SaveChanges();

            foreach (var membro in request.MuscleDays)
            {
                var membroTrabalhado = _appDbContext.GruposMuscularesTrabalhados
                .FirstOrDefault(x => x.IdTreino == treino.Id && x.GrupoMuscular == membro.MuscleGroup);

                if (membroTrabalhado == null)
                    throw new Exception("Erro ao salvar o exercício");

                var exerciciosDoTreino = membro.Exercises.Select(q => new ExercicioDoTreino
                {
                    IdExercicio = q.IdExercicio,
                    IdMembroTrabalhado = membroTrabalhado.Id,
                    Repeticao = q.Reps,
                    Serie = q.Sets
                });

                _appDbContext.ExerciciosDoTreino.AddRange(exerciciosDoTreino);
            }
            _appDbContext.SaveChanges();


            scope.Complete();
        }

        return Sucesso("Treino salvo com sucesso");

    }
}
