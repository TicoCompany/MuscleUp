using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.ViewModels.Treinos;
using MuscleUp.Dominio.ViewModels.Usuarios;

namespace MuscleUp.Web.Api;

public class TreinosController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;

    public TreinosController(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpPost]
    public IActionResult Salvar([FromBody] TreinoRequest request)
    {
        try
        {
            var treino = new Treino
            {
                Id = request.Id ?? 0,
                IdAcademia = UsuarioLogado.IdAcademia ?? 1,
                
                Nome = request.Nome,
                Divisao = request.Divisao,
                Publico = request.Publico,
                Tempo = request.Tempo,
            };

            return Sucesso("Treino salvo com suceso!");
        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }
}
