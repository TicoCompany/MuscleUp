using Microsoft.AspNetCore.Mvc;

namespace MuscleUp.Web.Controllers;

public class ContaController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
}
