using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.ViewModels.Usuarios;

namespace MuscleUp.Dominio.Usuarios;
public interface IUsuarioService
{
    ResultService<int?> Salvar(UsuarioRequest request);
}
internal class UsuarioService : IUsuarioService
{
    private readonly IAppDbContext _appDbContext;

    public UsuarioService(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public ResultService<int?> Salvar(UsuarioRequest request)
    {
        var usuarioDoBanco = _appDbContext.Usuarios.FirstOrDefault(q => q.Id == request.Id);


        var usuario = new Usuario
        {
            Id = request.Id ?? 0,
            Nome = request.Nome,
            Email = request.Email,
            Senha = request.Senha == null ? usuarioDoBanco!.Senha : request.Senha,
        };

        if (request.Id != null)
            _appDbContext.Usuarios.Update(usuario);
        else
            _appDbContext.Usuarios.Add(usuario);


        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "teste");
    }
}
