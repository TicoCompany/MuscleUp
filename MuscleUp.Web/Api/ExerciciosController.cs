using Microsoft.AspNetCore.Mvc;
using MuscleUp.Dominio.Filters;
using MuscleUp.Dominio.Pagination;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.ViewModels.Exercicios;
using CloudinaryDotNet;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Options;
using MuscleUp.Dominio.Cloudinary;
using CloudinaryDotNet.Actions;
using System.IO;
using MuscleUp.Dominio.Componentes;

namespace MuscleUp.Web.Api;

public class ExerciciosController : BaseApiController
{
    private readonly IExercicioService _exercicioService;
    private readonly Cloudinary _cloudinary;

    public ExerciciosController(IExercicioService exercicio, Cloudinary cloudinary, IOptions<CloudinarySettings> config)
    {
        _exercicioService = exercicio;
        var account = new Account(
           config.Value.CloudName,
           config.Value.ApiKey,
           config.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
        _cloudinary = cloudinary;
    }

    [HttpPost]
    public async Task<IActionResult> Salvar([FromForm] ExercicioRequest request, [FromForm] IFormFile? arquivo)
    {
        try
        {
            if (arquivo != null && arquivo.Length != 0)
            {
                using var stream = arquivo.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(arquivo.FileName, stream),
                    Folder = "exercicios"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception("Erro ao salvar imagem");

                request.Caminho = uploadResult.SecureUrl.ToString();
                request.PublicId = uploadResult.PublicId;
            }

            var result = _exercicioService.Salvar(request);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            if (arquivo != null && arquivo.Length != 0 && request.Id != 0)
            {
                var deletionParams = new DeletionParams(result.Dados);
                var response = await _cloudinary.DestroyAsync(deletionParams);
            }

            return Sucesso(result.Mensagem!);

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] ExercicioFilter filter)
    {
        try
        {
            if (UsuarioLogado.IdAcademia != 0)
                filter.IdAcademia = UsuarioLogado.IdAcademia;

            var result = _exercicioService.Listar(filter);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var paginedQuery = await PaginatedList<Exercicio>.CreateAsync(result.Dados!, filter.Pagina, filter.PorPagina);

            return Sucesso(new
            {
                Exercicios = paginedQuery.Items!.Select(q => new
                {
                    q.Nome,
                    q.Id,
                    q.Caminho,
                    NomeDaAcademia = q.Academia != null ? q.Academia.Nome : "-",
                    Dificuldade = q.Dificuldade.DisplayName(),
                    GrupoMuscular = q.GrupoMuscular.DisplayName()
                }),
                TotalPaginas = paginedQuery.TotalPages
            });

        }
        catch (Exception ex)
        {
            return Erro("Um erro inesperado aconteceu!");
        }
    }

    [HttpGet, Route("ListarPorMembroMuscular")]
    public async Task<IActionResult> ListarPorMembroMuscular([FromQuery] ExercicioFilter filter)
    {
        try
        {
            if (UsuarioLogado.IdAcademia != 0)
                filter.IdAcademia = UsuarioLogado.IdAcademia;

            var result = _exercicioService.ListarPorMembroMuscular(filter);
            if (!result.Sucesso)
                return Erro(result.Mensagem!);

            var paginedQuery = await PaginatedList<Exercicio>.CreateAsync(result.Dados!, filter.Pagina, filter.PorPagina);

            return Sucesso(new
            {
                Exercicios = paginedQuery.Items!.Select(q => new
                {
                    q.Nome,
                    q.Caminho,
                    q.Id,
                    Dificuldade = q.Dificuldade.DisplayName(),
                    GrupoMuscular = q.GrupoMuscular.DisplayName()
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
        var result = _exercicioService.Deletar(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(result.Mensagem!);
    }

    [HttpGet, Route("{id:int}")]
    public IActionResult BuscarPorId([FromRoute] int id)
    {
        var result = _exercicioService.BuscarPorId(id);

        if (!result.Sucesso)
            return Erro(result.Mensagem!);

        return Sucesso(new
        {
            result.Dados!.Nome,
            result.Dados!.Descricao,
            result.Dados!.Caminho,
            result.Dados!.PublicId,
            result.Dados!.Dificuldade,
            result.Dados!.GrupoMuscular,
            result.Dados!.IdAcademia,
            result.Dados!.Id,
        });
    }
}
