namespace MuscleUp.Dominio.Auth;

public class UsuarioSessaoModel
{
    public int Id { get; set; }
    public int? IdAcademia { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Perfil { get; set; }
}
