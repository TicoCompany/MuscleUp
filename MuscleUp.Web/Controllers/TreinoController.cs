using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.Exercicios.Enums;
using MuscleUp.Dominio.Treinos.Enums;

namespace MuscleUp.Web.Controllers;

public class TreinoController : BaseController
{
    public IActionResult Index()
    {
        ViewBag.IdAcademia = UsuarioLogado.IdAcademia;
        return View();
    }

    public IActionResult Create(int? id)
    {
        var divisoes = EnumExtensions.ToEnum<DivisaoDeTreino>();
        var gruposMusculares = EnumExtensions.ToEnumName<GrupoMuscular>();

        ViewBag.Json = new
        {
            Divisoes = divisoes,
            GruposMusculares = gruposMusculares.Select(q => new
            {
                grupoMuscular = q.EnumValue,
                nome = q.Nome,
            }),
            Id = id,
            IdAcademia = UsuarioLogado.IdAcademia
        };
        ViewBag.Id = id;
        return View();
    }
}
