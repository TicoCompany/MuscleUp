using MuscleUp.Dominio.Componentes;

namespace MuscleUp.Dominio.Mensageria;

public sealed record EmailSettings
{
    public string EmailRemetente { get; set; } = App.Email;
    public string NomeRemetente { get; set; } = App.Nome;
    public string SmtpHost { get; set; } = "smtp.gmail.com";
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsuario { get; set; } = "ticocompany3@gmail.com";
    public string SmtpSenha { get; set; } = "pzhu nhsh jxny acou";
}
