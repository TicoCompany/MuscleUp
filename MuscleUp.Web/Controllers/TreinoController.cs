using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.Exercicios.Enums;
using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Web.Controllers;

public class TreinoController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create(int? id)
    {
        var divisoes = EnumExtensions.ToEnum<DivisaoDeTreino>();
        var gruposMusculares = EnumExtensions.ToEnum<GrupoMuscular>();

        ViewBag.Json = new
        {
            Divisoes = divisoes,
            GruposMusculares = gruposMusculares,
            Id = id,
            IdAcademia = UsuarioLogado.IdAcademia
        };
        ViewBag.Id = id;
        return View();
    }
}
