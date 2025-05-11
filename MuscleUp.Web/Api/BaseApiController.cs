using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Auth;

namespace MuscleUp.Web.Api;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected UsuarioSessaoModel UsuarioLogado => HttpContext.ObterUsuarioLogado();

    protected OkObjectResult Sucesso(string mensagem)
    {
        return Ok(new
        {
            Sucesso = true,
            Mensagem = mensagem
        });
    }

    protected OkObjectResult Sucesso(object obj)
    {
        return Ok(new
        {
            Sucesso = true,
            Data = obj
        });
    }


    protected OkObjectResult Erro(string mensagem)
    {
        return Ok(new
        {
            Sucesso = false,
            Mensagem = mensagem
        });
    }
}