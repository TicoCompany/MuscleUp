using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Auth;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.ViewModels.Contas;
using System.Security.Claims;
namespace MuscleUp.Web.Api;

public class ContasController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;

    public ContasController(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = _appDbContext.Usuarios.FirstOrDefault(u => u.Email == request.Email);

        if (usuario == null || usuario.Senha != request.Senha)
            return Ok("Credenciais inválidas");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        };

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddHours(8)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync("Cookies", claimsPrincipal);

        var usuarioSessao = new UsuarioSessaoModel
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email
        };

        return Ok("Login realizado com sucesso!");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return Ok(new { message = "Logout realizado com sucesso" });
    }

}
