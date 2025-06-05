using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Academias;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Exercicios.Enums;

namespace MuscleUp.Web.Controllers
{
    public class ExercicioController : BaseController
    {
        private readonly IAppDbContext _appDbContext;

        public ExercicioController(IAppDbContext context)
        {
            _appDbContext = context;
        }

        public IActionResult Index()
        {
            var academias = _appDbContext.Academias.AsNoTracking();
            var gruposMusculares = Enum.GetValues(typeof(GrupoMuscular))
                .Cast<GrupoMuscular>()
                .Select(e => new
                {
                    Id = (int)e,
                    Nome = e.DisplayName()
                }).ToList().OrderBy(q => q.Nome);

            var dificuldades = Enum.GetValues(typeof(DificuldadeDoExercicio))
               .Cast<DificuldadeDoExercicio>()
               .Select(e => new
               {
                   Id = (int)e,
                   Nome = e.DisplayName()
               }).ToList();

            ViewBag.Json = new
            {
                Academias = academias.Select(q => new
                {
                    q.Id,
                    q.Nome
                }),
                GruposMusculares = gruposMusculares,
                Dificuldades = dificuldades,
                IdAcademia = UsuarioLogado.IdAcademia,
            };
            return View();
        }

        public IActionResult Create(int? id)
        {
            var usuarioLogado = UsuarioLogado;
            var gruposMusculares = Enum.GetValues(typeof(GrupoMuscular))
               .Cast<GrupoMuscular>()
               .Select(e => new
               {
                   Id = (int)e,
                   Nome = e.DisplayName()
               }).ToList().OrderBy(q => q.Nome);

            var dificuldades = Enum.GetValues(typeof(DificuldadeDoExercicio))
               .Cast<DificuldadeDoExercicio>()
               .Select(e => new
               {
                   Id = (int)e,
                   Nome = e.DisplayName()
               }).ToList();
            ViewBag.Json = new
            {
                IdUsuarioLogado = usuarioLogado.Id,
                IdAcademia = usuarioLogado.IdAcademia,
                Id = id == null ? 0 : id,
                GruposMusculares = gruposMusculares,
                Dificuldades = dificuldades,
            };
            return View(id);
        }
    }
}
