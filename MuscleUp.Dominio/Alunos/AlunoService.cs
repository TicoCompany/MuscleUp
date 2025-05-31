using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Mensageria;
using MuscleUp.Dominio.Usuarios;
using MuscleUp.Dominio.ViewModels.Alunos;

namespace MuscleUp.Dominio.Alunos;
public interface IAlunoService
{
    Task<ResultService<int?>> Salvar(AlunoRequest request);
    ResultService<IQueryable<Aluno>> Listar();
    ResultService<Aluno?> BuscarPorId(int id);
    ResultService<int?> Deletar(int id);
}
internal class AlunoService : IAlunoService
{
    private readonly IAppDbContext _appDbContext;
    private readonly IEnviadorDeEmail _enviadorDeEmail;

    public AlunoService(IAppDbContext appDbContext, IEnviadorDeEmail enviadorDeEmail)
    {
        _appDbContext = appDbContext;
        _enviadorDeEmail = enviadorDeEmail;
    }

    public async Task<ResultService<int?>> Salvar(AlunoRequest request)
    {
        var usuarioDoBanco = _appDbContext.Usuarios.AsNoTracking().FirstOrDefault(q => q.Id == request.Id);
        string? senhaNova = null;
        string? senhaEncriptografada = null;
        if (usuarioDoBanco == null)
        {
            senhaNova = GeradorSenha.GerarSenha();
            senhaEncriptografada = BCrypt.Net.BCrypt.HashPassword(senhaNova);
        }

        var usuario = new Usuario
        {
            Id = request.Id ?? 0,
            Nome = request.Nome,
            Email = request.Email,
            Senha = senhaEncriptografada != null ? senhaEncriptografada : usuarioDoBanco!.Senha,
        };

        if (request.Id != null)
            _appDbContext.Usuarios.Update(usuario);
        else
            _appDbContext.Usuarios.Add(usuario);

        _appDbContext.SaveChanges();

        var aluno = new Aluno
        {
            IdUsuario = usuario.Id,
            Altura = request.Altura,
            Objetivo = request.Objetivo,
            Peso = request.Peso,
            ProblemasMedicos = request.ProblemasMedicos,
        };

        if (request.Id != null)
            _appDbContext.Alunos.Update(aluno);
        else
            _appDbContext.Alunos.Add(aluno);

        _appDbContext.SaveChanges();

        if (!string.IsNullOrWhiteSpace(senhaNova))
        {

            var emailModel = new EmailModel
            {
                EmailDoDestinatario = usuario.Email,
                Nome = usuario.Nome,
            };
            await _enviadorDeEmail.EnviarSenhaParaAluno(emailModel, "MuscleUp", senhaNova);
        }


        return ResultService<int?>.Ok(null, "Aluno salvo com sucesso!");
    }

    public ResultService<IQueryable<Aluno>> Listar()
    {

        var alunos = _appDbContext.Alunos.Include(q => q.Usuario).AsNoTracking().AsQueryable();

        return ResultService<IQueryable<Aluno>>.Ok(alunos);
    }

    public ResultService<int?> Deletar(int id)
    {
        var usuario = _appDbContext.Usuarios.FirstOrDefault(q => q.Id == id);
        if (usuario == null)
            return ResultService<int?>.Falha("Aluno não encontrado");

        _appDbContext.Usuarios.Remove(usuario);
        _appDbContext.SaveChanges();

        return ResultService<int?>.Ok(null, "Aluno excluído com sucesso!");
    }

    public ResultService<Aluno?> BuscarPorId(int id)
    {

        var aluno = _appDbContext.Alunos.Include(q => q.Usuario).AsNoTracking().FirstOrDefault(q => q.IdUsuario == id);

        if (aluno == null)
            return ResultService<Aluno?>.Falha("Aluno não encontrado");

        return ResultService<Aluno?>.Ok(aluno);
    }
}
