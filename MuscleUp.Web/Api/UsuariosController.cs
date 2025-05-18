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

    [HttpGet]
    public IActionResult List()
    {
        try
        {
            var result = _usuarioService.Listar();
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            return Sucesso(result.Dados!.Select(q => new
            {
                q.Nome,
                q.Email,
                q.Id,
            }));

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpDelete, Route("{id:int}")]
    public IActionResult Excluir([FromRoute] int id)
    {
        var result = _usuarioService.Deletar(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(result.Mensagem!);
    }

    [HttpGet, Route("{id:int}")]
    public IActionResult BuscarPorId([FromRoute] int id)
    {
        var result = _usuarioService.BuscarPorId(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(new
        {
            result.Dados!.Email,
            result.Dados!.Nome,
            result.Dados!.Id,
        });
    }
}
