namespace MuscleUp.Dominio.ViewModels.Usuarios;

public sealed record UsuarioRequest
{
    public int? Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string? Senha { get; set; }

}
