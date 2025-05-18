using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Usuarios;
using MuscleUp.Dominio.ViewModels.Contas;
using MuscleUp.Dominio.ViewModels.Usuarios;

namespace MuscleUp.Dominio.Contas;
public interface IContaService
{
    ResultService<Usuario> Login(LoginRequest request);
}

internal class ContaService : IContaService
{
    private readonly IAppDbContext _appDbContext;

    public ContaService(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public ResultService<Usuario> Login(LoginRequest request)
    {
        var usuario = _appDbContext.Usuarios.AsNoTracking().FirstOrDefault(u => u.Email == request.Email);

        if (usuario == null)
            return ResultService<Usuario>.Falha("E-mail não cadastrado");

        bool senhaCorreta = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.Senha);

        if (!senhaCorreta)
            return ResultService<Usuario>.Falha("Senha inválida");


        return ResultService<Usuario>.Ok(usuario, "Login realizado com sucesso!");

    }
}
