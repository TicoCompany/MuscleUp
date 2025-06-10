using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.DataBase;

namespace MuscleUp.Web.Controllers;

public class AlunoController : BaseController
{
    private readonly IAppDbContext _context;

    public AlunoController(IAppDbContext context) => _context = context;

    public IActionResult Index()
    {
        ViewBag.Json = new
        {
            Academias = _context.Academias.AsNoTracking().OrderBy(q => q.Nome).Select(q => new
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
        ViewBag.Usuario = UsuarioLogado;
        ViewBag.Json = new
        {
            Academias = _context.Academias.AsNoTracking().OrderBy(q => q.Nome).Select(q => new { q.Id, q.Nome }),
            Id = id
        };

        return View();
    }

    public IActionResult VisualizarTreinosDoAluno(int id)
    {
        var aluno = _context.Alunos.Include(q => q.Usuario).AsNoTracking();

        ViewBag.Json = new
        {
            Id = id
        };

        return View(aluno);
    }
}
