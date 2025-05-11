using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Web.Models;

namespace MuscleUp.Web.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAppDbContext _appDbContext;

    public HomeController(ILogger<HomeController> logger, IAppDbContext appDbContext)
    {
        _logger = logger;
        _appDbContext = appDbContext;
    }

    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}    

