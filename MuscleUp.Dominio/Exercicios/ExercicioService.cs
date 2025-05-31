using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.ViewModels.Exercicios;

namespace MuscleUp.Dominio.Exercicios;
public interface IExercicioService
{
    ResultService<int?> Salvar(ExercicioRequest request);
    ResultService<IQueryable<Exercicio>> Listar(ExercicioFilter filter);
    ResultService<Exercicio?> BuscarPorId(int id);
    ResultService<int?> Deletar(int id);
}

public class ExercicioService : IExercicioService
{
    private readonly IAppDbContext _appDbContext;
    public ExercicioService(IAppDbContext appDbContext) => _appDbContext = appDbContext;

    public ResultService<int?> Salvar(ExercicioRequest request)
    {

        var exercicio = new Exercicio
        {
            Id = request.Id ?? 0,
            IdAcademia = request.IdAcademia,
            Nome = request.Nome,
        };

        if (request.Id != null)
            _appDbContext.Exercicios.Update(exercicio);
        else
            _appDbContext.Exercicios.Add(exercicio);

        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "Exercício salvo com sucesso!");
    }

    public ResultService<IQueryable<Exercicio>> Listar(ExercicioFilter filter)
    {
        var exercicio = _appDbContext.Exercicios.Include(q => q.Academia).AsNoTracking().AsQueryable();

        if (filter.IdAcademia.HasValue)
            exercicio = exercicio.Where(q => q.IdAcademia == filter.IdAcademia);

        if (!string.IsNullOrWhiteSpace(filter.Busca))
            exercicio = exercicio.Where(q => q.Nome.Contains(filter.Busca));

        return ResultService<IQueryable<Exercicio>>.Ok(exercicio);
    }

    public ResultService<int?> Deletar(int id)
    {

        var Exercicio = _appDbContext.Exercicios.FirstOrDefault(q => q.Id == id);

        if (Exercicio == null)
            return ResultService<int?>.Falha("Exercício não encontrado");

        _appDbContext.Exercicios.Remove(Exercicio);
        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "Exercício excluído com sucesso!");
    }

    public ResultService<Exercicio?> BuscarPorId(int id)
    {

        var exercicio = _appDbContext.Exercicios.AsNoTracking().FirstOrDefault(q => q.Id == id);

        if (exercicio == null)
            return ResultService<Exercicio?>.Falha("Exercício não encontrado");

        return ResultService<Exercicio?>.Ok(exercicio);
    }
}
