using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Usuarios;
using MuscleUp.Dominio.ViewModels.Usuarios;

namespace MuscleUp.Web.Api;

public class UsuariosController : BaseApiController
{

    private readonly IAppDbContext _appDbContext;
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IAppDbContext appDbContext, IUsuarioService usuarioService)
    {
        _appDbContext = appDbContext;
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public IActionResult Salvar([FromBody] UsuarioRequest request)
    {
        try
        {
            var result = _usuarioService.Salvar(request);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            return Sucesso("Usuário salvo com suceso!");

        }
        catch (Exception ex) 
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }
}
