using Microsoft.AspNetCore.Mvc;

namespace MuscleUp.Web.Controllers;

public class UsuarioController : BaseController
{
    public IActionResult Index() => View();
    public IActionResult Create(int? id) => View(id);

}
