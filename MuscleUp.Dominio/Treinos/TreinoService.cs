using MailKit;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Auth;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.GruposMuscularesTrabalhados;
using MuscleUp.Dominio.Treinos.Enums;
using MuscleUp.Dominio.ViewModels.Treinos;

namespace MuscleUp.Dominio.Treinos;
public interface ITreinoService
{
    ResultService<List<DivisoesDeTreinoResponse>> SalvarStep1(TreinoRequest request, UsuarioSessaoModel usuarioLogado);
    ResultService<List<Step2Response>> SalvarStep2(TreinoRequest request);
    ResultService<int?> SalvarStep3(TreinoRequest request);
    ResultService<int?> EnviarParaAlunos(EnviarTreinoParaAluno request, int? IdProfessor);
    ResultService<IQueryable<Treino>> Listar(TreinoFilter filter);
    ResultService<Treino?> BuscarPorId(int id);
    ResultService<int?> Deletar(int id);
    ResultService<int?> DeletarGrupoMuscular(int id);
}
internal class TreinoService : ITreinoService
{
    private readonly IAppDbContext _appDbContext;

    public TreinoService(IAppDbContext appDbContext) => _appDbContext = appDbContext;
    public ResultService<List<DivisoesDeTreinoResponse>> SalvarStep1(TreinoRequest request, UsuarioSessaoModel usuarioLogado)
    {
        var treino = new Treino
        {
            Id = request.Id ?? 0,
            IdProfessor = usuarioLogado.Id,
            IdAcademia = usuarioLogado.IdAcademia ?? 1,
            Nome = request.Nome,
            Divisao = request.Divisao,
            Publico = request.Publico,
            Tempo = request.Tempo,
            DificuldadeDoTreino = request.DificuldadeDoTreino,
        };

        if (!request.Id.HasValue)
            _appDbContext.Treinos.Add(treino);
        else
            _appDbContext.Treinos.Update(treino);

        _appDbContext.SaveChanges();


        var divisoesDeSubTreino = ObterDivisoes(treino.Divisao);
        var divisoes = divisoesDeSubTreino.Select(q => new DivisoesDeTreinoResponse

        {
            DivisaoDeSubTreino = (int)q,
            NomeDaDivisao = q.DisplayName(),
            Membros = new List<object>(),
            IdTreino = treino.Id
        }).ToList();

        return ResultService<List<DivisoesDeTreinoResponse>>.Ok(divisoes, "Informações do treino salvas com sucesso!");
    }

    public ResultService<List<Step2Response>> SalvarStep2(TreinoRequest request)
    {
        var treino = _appDbContext.Treinos.FirstOrDefault(q => q.Id == request.Id);
        if (treino == null)
            return ResultService<List<Step2Response>>.Falha("Treino não encontrado");


        List<GrupoMuscularTrabalhado> membrosMusculares = new List<GrupoMuscularTrabalhado>();
        foreach (var divisao in request.Divisoes)
        {
            foreach (var membro in divisao.Membros)
            {
                var membroTrabalhado = new GrupoMuscularTrabalhado
                {
                    Id = membro.Id ?? 0,
                    GrupoMuscular = membro.GrupoMuscular,
                    IdTreino = request.Id ?? throw new Exception("Ocorreu um erro inesperado"),
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

        var membrosSalvos = membrosMusculares.Select(q => new Step2Response
        {
            GrupoMuscular = (int)q.GrupoMuscular,
            DivisaoDeSubTreino = (int)q.DivisaoDeTreino,
            IdDoMembro = q.Id,
        }).ToList();

        return ResultService<List<Step2Response>>.Ok(membrosSalvos, "Grupos musculares salvos com sucesso!");
    }

    public ResultService<int?> SalvarStep3(TreinoRequest request)
    {
        var treino = _appDbContext.Treinos.FirstOrDefault(q => q.Id == request.Id);
        if (treino == null)
            return ResultService<int?>.Falha("Treino salvo com sucesso!");

        foreach (var divisao in request.Divisoes)
        {
            foreach (var membro in divisao.Membros)
            {
                var exerciciosDoGrupo = _appDbContext.ExerciciosDoTreino.Where(q => q.IdMembroTrabalhado == membro.Id);
                _appDbContext.ExerciciosDoTreino.RemoveRange(exerciciosDoGrupo);

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

                    _appDbContext.ExerciciosDoTreino.Add(exercicio);
                }
            }
        }

        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "Treino salvo com sucesso!");
    }

    public ResultService<IQueryable<Treino>> Listar(TreinoFilter filter)
    {
        var treinos = _appDbContext.Treinos.Include(q => q.Academia).AsNoTracking().Where(q => q.Aluno == null).AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Busca))
            treinos = treinos.Where(q => q.Nome.Contains(filter.Busca));

        if (filter.IdAcademia != 0)
            treinos = treinos.Where(q => q.IdAcademia == filter.IdAcademia);


        return ResultService<IQueryable<Treino>>.Ok(treinos);
    }

    public ResultService<int?> Deletar(int id)
    {

        var treino = _appDbContext.Treinos.FirstOrDefault(q => q.Id == id);

        if (treino == null)
            return ResultService<int?>.Falha("Treino não encontrado");

        _appDbContext.Treinos.Remove(treino);
        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "Treino excluído com sucesso!");
    }

    public ResultService<Treino?> BuscarPorId(int id)
    {

        var treino = _appDbContext.Treinos.Include(q => q.GruposMuscularesTrabalhados).ThenInclude(q => q.ExerciciosDoTreino).ThenInclude(q => q.Exercicio).AsNoTracking().FirstOrDefault(q => q.Id == id);

        if (treino == null)
            return ResultService<Treino?>.Falha("Treino não encontrado");

        return ResultService<Treino?>.Ok(treino);
    }

    public ResultService<int?> DeletarGrupoMuscular(int id)
    {

        var grupoMuscular = _appDbContext.GruposMuscularesTrabalhados.FirstOrDefault(q => q.Id == id);

        if (grupoMuscular == null)
            return ResultService<int?>.Falha("Grupo muscular encontrado");

        _appDbContext.GruposMuscularesTrabalhados.Remove(grupoMuscular);
        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "Grupo muscular excluído com sucesso!");
    }

    public ResultService<int?> EnviarParaAlunos(EnviarTreinoParaAluno request, int? idProfessor)
    {
        foreach (var id in request.IdsDosALunos)
        {
            var treinoDoBanco = _appDbContext.TreinosPublicosEDestinadosDoAluno.AsNoTracking().FirstOrDefault(q => q.Id == id && q.IdTreino == request.IdTreino);
            if (treinoDoBanco != null)
                continue;

            var treinoDestinado = new TreinoPublicoEDestinadoDoAluno
            {
                IdAluno = id,
                IdProfessorQueDestinou = idProfessor,
                IdTreino = request.IdTreino,
            };

            _appDbContext.TreinosPublicosEDestinadosDoAluno.Add(treinoDestinado);
        }

        _appDbContext.SaveChanges();
        return ResultService<int?>.Ok(null, "Treino enviado com sucesso!");
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
