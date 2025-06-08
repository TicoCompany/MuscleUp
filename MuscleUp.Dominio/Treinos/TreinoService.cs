using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.ViewModels.Treinos;

namespace MuscleUp.Dominio.Treinos;
public interface ITreinoService
{
    ResultService<int?> Salvar(TreinoRequest request, int idTreinoLogado);
    ResultService<IQueryable<Treino>> Listar(TreinoFilter filter);
    ResultService<Treino?> BuscarPorId(int id);
    ResultService<int?> Deletar(int id);
    ResultService<int?> DeletarGrupoMuscular(int id);
}
internal class TreinoService : ITreinoService
{
    private readonly IAppDbContext _appDbContext;

    public TreinoService(IAppDbContext appDbContext) => _appDbContext = appDbContext;

    public ResultService<int?> Salvar(TreinoRequest request, int idTreinoLogado)
    {

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
}
