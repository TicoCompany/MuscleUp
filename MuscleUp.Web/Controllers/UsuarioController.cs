using Microsoft.AspNetCore.Mvc;

namespace MuscleUp.Web.Controllers;

public class UsuarioController : BaseController
{
    public IActionResult Index()
    {
        var usuario = UsuarioLogado;
        return View();
    }

    public IActionResult Create(int? id) => View(id);

}
