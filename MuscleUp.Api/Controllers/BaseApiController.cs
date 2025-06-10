using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Auth;

namespace MuscleUp.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class BaseApiController : ControllerBase
{
    protected UsuarioSessaoModel UsuarioLogado => HttpContext.ObterUsuarioLogado();
}
