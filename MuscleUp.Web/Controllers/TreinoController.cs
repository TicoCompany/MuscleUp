using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Exercicios.Enums;
using MuscleUp.Dominio.Treinos.Enums;
using MuscleUp.Dominio.ViewModels.Treinos;

namespace MuscleUp.Web.Controllers;

public class TreinoController : BaseController
{
    private readonly IAppDbContext _appDbContext;

    public TreinoController(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public IActionResult Index()
    {
        var divisoes = EnumExtensions.ToEnum<DivisaoDeTreino>();
        var gruposMusculares = EnumExtensions.ToEnum<GrupoMuscular>();
        var dificuldades = EnumExtensions.ToEnum<DificuldadeDoTreino>();

        ViewBag.Json = new
        {
            Divisoes = divisoes,
            GruposMusculares = gruposMusculares,
            Dificuldades = dificuldades,
            IdAcademia = UsuarioLogado.IdAcademia
        };

        ViewBag.IdAcademia = UsuarioLogado.IdAcademia;
        return View();
    }

    public IActionResult Create(int? id)
    {
        var divisoes = EnumExtensions.ToEnum<DivisaoDeTreino>();
        var gruposMusculares = EnumExtensions.ToEnumName<GrupoMuscular>();
        var dificuldades = EnumExtensions.ToEnum<DificuldadeDoTreino>();

        ViewBag.Json = new
        {
            Divisoes = divisoes,
            GruposMusculares = gruposMusculares.Select(q => new
            {
                grupoMuscular = q.EnumValue,
                nome = q.Nome,
            }),
            Dificuldades = dificuldades,
            Id = id,
            IdAcademia = UsuarioLogado.IdAcademia
        };
        ViewBag.Id = id;
        return View();
    }

    public IActionResult Visualizar(int id)
    {
        var treino = _appDbContext.Treinos
            .AsNoTracking()
            .Include(q => q.GruposMuscularesTrabalhados)
            .ThenInclude(q => q.ExerciciosDoTreino)
            .ThenInclude(q => q.Exercicio)
            .FirstOrDefault(q => q.Id == id);

        if (treino == null)
            return NotFound();

        var response = new TreinoResponse
        {
            Id = treino.Id,
            DificuldadeDoTreino = treino.DificuldadeDoTreino.DisplayName(),
            Publico = treino.Publico,
            NomeDaDivisao = treino.Divisao.DisplayName(),
            Tempo = treino.Tempo,
            Nome = treino.Nome,
            Divisoes = treino.GruposMuscularesTrabalhados.GroupBy(q => q.DivisaoDeTreino).Select(g => new DivisoesDoTreinoResponse
            {
                NomeDaDivisaoDoSubTreino = ((DivisaoDeSubTreino)g.Key).DisplayName(),
                Membros = g.Select(v => new MembroMusculareResponse
                {
                    Id = v.Id,
                    NomeDoGrupoMuscular = v.GrupoMuscular.DisplayName(),
                    Exercicios = v.ExerciciosDoTreino.Select(e => new ExercicioDoTreinoResponse
                    {
                        Id = e.Id,
                        IdExercicio = e.IdExercicio,
                        Nome = e.Exercicio.Nome,
                        Serie = e.Serie,
                        Repeticao = e.Repeticao,
                        ImagemDoExercicio = e.Exercicio.Caminho,
                    }).ToList(),
                }).ToList()
            }).ToList()
        };

        return View(response);
    }

}
