using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MuscleUp.Dominio.Mensageria;

public interface IEmailService
{
    Task EnviarEmailAsync(EmailSettings emailSettings, string para, string assunto, string corpoHtml);
}
public class EmailService : IEmailService
{
    public async Task EnviarEmailAsync(EmailSettings emailSettings, string para, string assunto, string corpoHtml)
    {
        var mensagem = new MimeMessage();
        mensagem.From.Add(new MailboxAddress(emailSettings.NomeRemetente, emailSettings.EmailRemetente));
        mensagem.To.Add(MailboxAddress.Parse(para));
        mensagem.Subject = assunto;

        mensagem.Body = new TextPart("html")
        {
            Text = corpoHtml
        };

        using var cliente = new SmtpClient();

        await cliente.ConnectAsync(emailSettings.SmtpHost, emailSettings.SmtpPort, SecureSocketOptions.StartTls);

        if (!string.IsNullOrEmpty(emailSettings.SmtpUsuario))
        {
            await cliente.AuthenticateAsync(emailSettings.SmtpUsuario, emailSettings.SmtpSenha);
        }

        await cliente.SendAsync(mensagem);
        await cliente.DisconnectAsync(true);
    }
}
