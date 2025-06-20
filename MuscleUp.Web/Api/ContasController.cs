﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Contas;
using MuscleUp.Dominio.ViewModels.Contas;
namespace MuscleUp.Web.Api;

public class ContasController : BaseApiController
{
    private readonly IContaService _contasService;

    public ContasController(IContaService contasService)
    {
        _contasService = contasService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = _contasService.Login(request);

        if (!response.Sucesso)
            return Erro(response.Mensagem!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, response.Dados!.Nome),
            new Claim(ClaimTypes.Email, response.Dados!.Email),
            new Claim(ClaimTypes.NameIdentifier, response.Dados!.Id.ToString()),
             new Claim("IdAcademia", response.Dados!.IdAcademia?.ToString() ?? string.Empty)
        };

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddHours(8)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync("Cookies", claimsPrincipal);

        return Sucesso("Login realizado com sucesso!");
    }

}
