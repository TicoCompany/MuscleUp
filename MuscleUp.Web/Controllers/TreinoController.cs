using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Componentes;
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

        ViewBag.Json = new
        {
            Divisoes = divisoes,
            Id = id,
            IdAcademia = UsuarioLogado.IdAcademia
        };
        ViewBag.Id = id;
        return View();
    }
}
