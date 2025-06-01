using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.DataBase;

namespace MuscleUp.Web.Controllers;

public class ProfessorController : BaseController
{
    private readonly IAppDbContext _appDbContext;

    public ProfessorController(IAppDbContext appDbContext) => _appDbContext = appDbContext;

    public IActionResult Index()
    {
        var academias = _appDbContext.Academias.AsNoTracking();

        ViewBag.Json = new
        {
            Academias = academias.Select(q => new
            {
                q.Id,
                q.Nome
            }),
            IdAcademia = UsuarioLogado.IdAcademia
        };

        return View();
    }

    public IActionResult Create(int? id)
    {
        var academias = _appDbContext.Academias.AsNoTracking();

        ViewBag.Json = new
        {
            Academias = academias.Select(q => new
            {
                q.Id,
                q.Nome
            }),
            Id = id,
            IdAcademia = UsuarioLogado.IdAcademia
        };

        return View();
    }
}
