using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.Contas;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.Mensageria;
using MuscleUp.Dominio.Usuarios;
using MuscleUp.Dominio.ViewModels.Alunos;
using MuscleUp.Dominio.ViewModels.Contas;

namespace MuscleUp.Dominio.Alunos;
public interface IAlunoService
{
    Task<ResultService<int?>> Salvar(AlunoRequest request, int idUsuarioLogado);
    ResultService<IQueryable<Aluno>> Listar(AlunoFilter filter);
    ResultService<Aluno?> BuscarPorId(int id);
    ResultService<int?> Deletar(int id);
}
internal class AlunoService : IAlunoService
{
    private readonly IAppDbContext _appDbContext;
    private readonly IEnviadorDeEmail _enviadorDeEmail;
    private readonly IContaService _contaService;

    public AlunoService(IAppDbContext appDbContext, IEnviadorDeEmail enviadorDeEmail, IContaService contaService)
    {
        _appDbContext = appDbContext;
        _enviadorDeEmail = enviadorDeEmail;
        _contaService = contaService;
    }

    public async Task<ResultService<int?>> Salvar(AlunoRequest request, int idUsuarioLogado)
    {   
        var usuarioDoBanco = _appDbContext.Usuarios.AsNoTracking().FirstOrDefault(q => q.Id == request.Id);
        string? senhaNova = null;
        string? senhaEncriptografada = null;

        if(_contaService.EmailJaExistente(new ValidarEmailRequest(request.Email, idUsuarioLogado, usuarioDoBanco)))
            return ResultService<int?>.Falha("E-mail já cadastrado!");

        if (usuarioDoBanco == null)
        {
            senhaNova = GeradorSenha.GerarSenha();
            senhaEncriptografada = BCrypt.Net.BCrypt.HashPassword(senhaNova);
        }

        var usuario = new Usuario
        {
            Id = request.Id ?? 0,
            IdAcademia = request.IdAcademia,
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

    public ResultService<IQueryable<Aluno>> Listar(AlunoFilter filter)
    {

        var alunos = _appDbContext.Alunos.Include(q => q.Usuario.Academia).Where(q => q.Usuario.IdAcademia != null).AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Busca))
            alunos = alunos.Where(q => q.Usuario.Nome.Contains(filter.Busca) || q.Usuario.Email.Contains(filter.Busca));

        if (filter.IdAcademia != 0)
            alunos = alunos.Where(q => q.Usuario.IdAcademia == filter.IdAcademia);

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
