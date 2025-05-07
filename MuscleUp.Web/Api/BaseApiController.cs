using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Auth;

namespace MuscleUp.Web.Api;
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected UsuarioSessaoModel UsuarioLogado => HttpContext.ObterUsuarioLogado();
}