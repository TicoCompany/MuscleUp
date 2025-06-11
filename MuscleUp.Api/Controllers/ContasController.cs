using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MuscleUp.Api.Configurations;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.ViewModels.Contas;

namespace MuscleUp.Api.Controllers;

public class ContasController : BaseApiController
{
    private readonly IAppDbContext _appDbContext;

    public ContasController(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(LoginRequest request)
    {
        var usuario = _appDbContext.Usuarios.Include(q => q.Academia).AsNoTracking().FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower() && u.Aluno != null);

        if (usuario == null)
            return Erro("E-mail não encontrado");

        bool senhaCorreta = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.Senha);

        if (!senhaCorreta)
            return Erro("Senha inválida");

        var token = TokenService.GenerateToken(usuario);

        return Sucesso(new
        {
            usuario.Id,
            NomeDaAcademia = usuario.Academia?.Nome,
            usuario.Nome,
            usuario.Email,
            Token = token
        });
    }

}
