﻿using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.Pagination;
using MuscleUp.Dominio.Professores;
using MuscleUp.Dominio.Usuarios;
using MuscleUp.Dominio.ViewModels.Professores;

namespace MuscleUp.Web.Api;

public class ProfessoresController : BaseApiController
{

    private readonly IProfessorService _professorService;

    public ProfessoresController(IProfessorService professorService)
    {
        _professorService = professorService;
    }

    [HttpPost]
    public IActionResult Salvar([FromBody] ProfessorRequest request)
    {
        try
        {
            if (!request.IdAcademia.HasValue)
                request.IdAcademia = UsuarioLogado.IdAcademia;

            var result = _professorService.Salvar(request, UsuarioLogado.Id);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);


            return Sucesso(result.Mensagem!);

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ProfessorFilter filter)
    {
        try
        {
            var result = _professorService.Listar(filter);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var paginedQuery = await PaginatedList<Usuario>.CreateAsync(result.Dados!, filter.Pagina, filter.PorPagina);

            return Sucesso(new
            {   
                Professores = paginedQuery.Items!.Select(q => new
                {
                    q.Nome,
                    q.Email,
                    q.Id,
                    nomeDaAcademia = q.Academia!.Nome,
                }),
                TotalPaginas = paginedQuery.TotalPages
            });

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpDelete, Route("{id:int}")]
    public IActionResult Excluir([FromRoute] int id)
    {
        var result = _professorService.Deletar(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(result.Mensagem!);
    }

    [HttpGet, Route("{id:int}")]
    public IActionResult BuscarPorId([FromRoute] int id)
    {
        var result = _professorService.BuscarPorId(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(new
        {
            result.Dados!.Email,
            result.Dados!.Nome,
            result.Dados!.Id,
        });
    }
}
