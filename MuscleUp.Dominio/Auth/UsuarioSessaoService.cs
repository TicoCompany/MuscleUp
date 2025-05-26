using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MuscleUp.Dominio.Auth;

public static class UsuarioSessaoService
{
    public static UsuarioSessaoModel? ObterUsuarioLogado(this HttpContext context)
    {
        var claims = context?.User?.Claims;
        if (claims == null || !claims.Any()) return null;
        var idAcademiaClaim = context?.User?.FindFirst("IdAcademia")?.Value;
        int.TryParse(idAcademiaClaim, out var idAcademia);

        return new UsuarioSessaoModel
        {
            Id = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0"),
            Nome = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
            Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            IdAcademia = idAcademia
        };
    }
}
    