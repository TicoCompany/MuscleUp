using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.ViewModels.Usuarios;

namespace MuscleUp.Dominio.Usuarios;
public interface IUsuarioService
{
    ResultService<int?> Salvar(UsuarioRequest request);
    ResultService<IQueryable<Usuario>> Listar();
    ResultService<Usuario?> BuscarPorId(int id);
    ResultService<int?> Deletar(int id);

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
        var usuarioDoBanco = _appDbContext.Usuarios.AsNoTracking().FirstOrDefault(q => q.Id == request.Id);

        string hash = "";
        if (request.Senha != null)
            hash = BCrypt.Net.BCrypt.HashPassword(request.Senha);

        var usuario = new Usuario
        {
            Id = request.Id ?? 0,
            Nome = request.Nome,
            Email = request.Email,
            Senha = request.Senha == null ? usuarioDoBanco!.Senha : hash,
        };

        if (request.Id != null)
            _appDbContext.Usuarios.Update(usuario);
        else
            _appDbContext.Usuarios.Add(usuario);


        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "Usuário salvo com sucesso!");
    }

    public ResultService<IQueryable<Usuario>> Listar()
    {

        var usuarios = _appDbContext.Usuarios.AsNoTracking().AsQueryable();

        return ResultService<IQueryable<Usuario>>.Ok(usuarios, "Usuário salvo com sucesso!");
    }

    public ResultService<int?> Deletar(int id)
    {

        var usuario = _appDbContext.Usuarios.FirstOrDefault(q => q.Id == id);

        if (usuario == null)
            return ResultService<int?>.Falha("Usuário não encontrado");

        _appDbContext.Usuarios.Remove(usuario);
        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "Usuário excluído com sucesso!");
    }

    public ResultService<Usuario?> BuscarPorId(int id)
    {

        var usuario = _appDbContext.Usuarios.AsNoTracking().FirstOrDefault(q => q.Id == id);

        if (usuario == null)
            return ResultService<Usuario?>.Falha("Usuário não encontrado");

        return ResultService<Usuario?>.Ok(usuario, "Usuário excluído com sucesso!");
    }
}
