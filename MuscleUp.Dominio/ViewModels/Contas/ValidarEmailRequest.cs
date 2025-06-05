using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio.ViewModels.Contas;

public sealed record ValidarEmailRequest(string email, int id, Usuario? usuario)
{
    public string Email { get; set; } = email;
    public int IdUsuarioLogado { get; set; } = id;
    public Usuario? Usuario { get; set; } = usuario;
}
