namespace MuscleUp.Dominio.ViewModels.Contas;

public sealed record LoginRequest
{
    public string Email {  get; set; }
    public string Senha {  get; set; }

}
