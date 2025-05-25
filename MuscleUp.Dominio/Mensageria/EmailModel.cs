namespace MuscleUp.Dominio.Mensageria;

public sealed record EmailModel
{
    public string EmailDoDestinatario { get; set; }
    public string Nome { get; set; }
    public EmailSettings Settings { get; set; } = new EmailSettings();
}
