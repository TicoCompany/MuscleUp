using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.ViewModels.Exercicios;

namespace MuscleUp.Dominio.Exercicios;
public interface IExercicioService
{
    ResultService<string?> Salvar(ExercicioRequest request);
    ResultService<IQueryable<Exercicio>> Listar(ExercicioFilter filter);
    ResultService<IQueryable<Exercicio>> ListarPorMembroMuscular(ExercicioFilter filter);
    ResultService<Exercicio?> BuscarPorId(int id);
    ResultService<int?> Deletar(int id);
}

public class ExercicioService : IExercicioService
{
    private readonly IAppDbContext _appDbContext;
    public ExercicioService(IAppDbContext appDbContext) => _appDbContext = appDbContext;

    public ResultService<string?> Salvar(ExercicioRequest request)
    {
        var caminhoDaImagemDoExercicio = _appDbContext.Exercicios.AsNoTracking().FirstOrDefault(q => q.Id == request.Id)?.PublicId;

        var exercicio = new Exercicio
        {
            Id = request.Id,
            IdAcademia = request.IdAcademia != 0 ? request.IdAcademia : null,
            Nome = request.Nome,
            PublicId = request.PublicId,
            Caminho = request.Caminho,
            Descricao = request.Descricao,
            Dificuldade = request.Dificuldade
        };

        if (request.Id != 0)
            _appDbContext.Exercicios.Update(exercicio);
        else
            _appDbContext.Exercicios.Add(exercicio);

        _appDbContext.SaveChanges();

        return ResultService<string?>.Ok(caminhoDaImagemDoExercicio, "Exercício salvo com sucesso!");
    }

    public ResultService<IQueryable<Exercicio>> Listar(ExercicioFilter filter)
    {
        var exercicio = _appDbContext.Exercicios.Include(q => q.Academia).AsNoTracking().AsQueryable();

        if (filter.IdAcademia.HasValue)
            exercicio = exercicio.Where(q => q.IdAcademia == filter.IdAcademia);

        if (!string.IsNullOrWhiteSpace(filter.Busca))
            exercicio = exercicio.Where(q => q.Nome.Contains(filter.Busca));

        if (filter.GrupoMuscular.HasValue)
            exercicio = exercicio.Where(q => q.GrupoMuscular == filter.GrupoMuscular);

        if (filter.Dificuldade.HasValue)
            exercicio = exercicio.Where(q => q.Dificuldade == filter.Dificuldade);


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

    public ResultService<IQueryable<Exercicio>> ListarPorMembroMuscular(ExercicioFilter filter)
    {
        var exercicio = _appDbContext.Exercicios.Include(q => q.Academia).AsNoTracking().AsQueryable();

        if (filter.IdAcademia.HasValue)
            exercicio = exercicio.Where(q => q.IdAcademia == filter.IdAcademia || q.IdAcademia == null);

        if (!string.IsNullOrWhiteSpace(filter.Busca))
            exercicio = exercicio.Where(q => q.Nome.Contains(filter.Busca));

        if (filter.GrupoMuscular.HasValue)
            exercicio = exercicio.Where(q => q.GrupoMuscular == filter.GrupoMuscular);

        if (filter.Dificuldade.HasValue)
            exercicio = exercicio.Where(q => q.Dificuldade == filter.Dificuldade);


        return ResultService<IQueryable<Exercicio>>.Ok(exercicio);
    }
}
