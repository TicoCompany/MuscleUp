using Microsoft.AspNetCore.Authentication;
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

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToAction("Login", "Conta");
    }
}
