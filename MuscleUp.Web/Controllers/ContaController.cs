using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MuscleUp.Web.Controllers;

public class ContaController : BaseController
{
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }
}
