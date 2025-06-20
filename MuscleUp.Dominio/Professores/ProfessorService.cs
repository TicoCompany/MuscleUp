﻿using Microsoft.EntityFrameworkCore;
using MuscleUp.Dominio.Componentes;
using MuscleUp.Dominio.Contas;
using MuscleUp.Dominio.DataBase;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.Mensageria;
using MuscleUp.Dominio.Usuarios;
using MuscleUp.Dominio.ViewModels.Contas;
using MuscleUp.Dominio.ViewModels.Professores;

namespace MuscleUp.Dominio.Professores;

public interface IProfessorService
{
    ResultService<int?> Salvar(ProfessorRequest request, int idUsuarioLogado);
    ResultService<IQueryable<Usuario>> Listar(ProfessorFilter filter);
    ResultService<Usuario?> BuscarPorId(int id);
    ResultService<int?> Deletar(int id);
}
internal class ProfessorService : IProfessorService
{
        private readonly IAppDbContext _appDbContext;
        private readonly IEnviadorDeEmail _enviadorDeEmail;
        private readonly IContaService _contaService;

        public ProfessorService(IAppDbContext appDbContext, IEnviadorDeEmail enviadorDeEmail, IContaService contaService)
        {
            _appDbContext = appDbContext;
            _enviadorDeEmail = enviadorDeEmail;
            _contaService = contaService;
        }

        public ResultService<int?> Salvar(ProfessorRequest request, int idUsuarioLogado)
        {
            var usuarioDoBanco = _appDbContext.Usuarios.AsNoTracking().FirstOrDefault(q => q.Id == request.Id);

            if (_contaService.EmailJaExistente(new ValidarEmailRequest(request.Email, idUsuarioLogado, usuarioDoBanco)))
                return ResultService<int?>.Falha("E-mail já cadastrado!");

            string hash = "";
            if (request.Senha != null)
                hash = BCrypt.Net.BCrypt.HashPassword(request.Senha);


            var usuario = new Usuario
            {
                Id = request.Id ?? 0,
                Nome = request.Nome,
                Email = request.Email,
                Senha = hash != null ? hash : usuarioDoBanco!.Senha,
                IdAcademia = request.IdAcademia
            };

            if (request.Id != null)
                _appDbContext.Usuarios.Update(usuario);
            else
                _appDbContext.Usuarios.Add(usuario);

            _appDbContext.SaveChanges();

            return ResultService<int?>.Ok(null, "Professor salvo com sucesso!");
        }

        public ResultService<IQueryable<Usuario>> Listar(ProfessorFilter filter)
        {
            var professores = _appDbContext.Usuarios.Include(q => q.Academia).AsNoTracking().Where(q => q.IdAcademia != null && q.Aluno == null).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Busca))
                professores = professores.Where(q => q.Nome.Contains(filter.Busca) || q.Email.Contains(filter.Busca));

            if (filter.IdAcademia != 0)
                professores = professores.Where(q => q.IdAcademia == filter.IdAcademia);


            return ResultService<IQueryable<Usuario>>.Ok(professores);
        }

        public ResultService<int?> Deletar(int id)
        {

            var professor = _appDbContext.Usuarios.FirstOrDefault(q => q.Id == id);

            if (professor == null)
                return ResultService<int?>.Falha("Professor não encontrado");

            _appDbContext.Usuarios.Remove(professor);
            _appDbContext.SaveChanges();

            return ResultService<int?>.Ok(null, "Professor excluído com sucesso!");
        }

        public ResultService<Usuario?> BuscarPorId(int id)
        {

            var professor = _appDbContext.Usuarios.AsNoTracking().FirstOrDefault(q => q.Id == id);

            if (professor == null)
                return ResultService<Usuario?>.Falha("Professor não encontrado");

            return ResultService<Usuario?>.Ok(professor);
        }
}