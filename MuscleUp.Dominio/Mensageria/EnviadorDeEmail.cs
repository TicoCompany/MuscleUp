using MuscleUp.Dominio.Componentes;

namespace MuscleUp.Dominio.Mensageria;
public interface IEnviadorDeEmail
{
    Task EnviarSenhaParaAluno(EmailModel model, string nomeDaAcademia, string senha);
}
internal class EnviadorDeEmail : IEnviadorDeEmail
{

    private readonly IEmailService _emailService;

    public EnviadorDeEmail(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task EnviarSenhaParaAluno(EmailModel model, string nomeDaAcademia, string senha)
    {
        var assunto = $"Bem-vindo à {nomeDaAcademia}!";

        var corpo = $@"
<!DOCTYPE html>
<html lang='pt-br'>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f6f6f6;
            color: #333;
            padding: 20px;
        }}
        .container {{
            background-color: #fff;
            max-width: 600px;
            margin: auto;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.05);
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
        }}
        .header h1 {{
            color: #2b7a78;
            margin: 0;
            font-size: 28px;
        }}
        .content {{
            font-size: 16px;
            line-height: 1.6;
        }}
        .senha-box {{
            background-color: #eafaf1;
            border: 1px dashed #2b7a78;
            padding: 15px;
            font-weight: bold;
            text-align: center;
            margin: 20px 0;
        }}
        .logo {{
            text-align: center;
            margin-top: 30px;
        }}
        .logo img {{
            max-width: 180px;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>{nomeDaAcademia}</h1>
        </div>
        <div class='content'>
            <p>Olá <strong>{model.Nome}</strong>,</p>
            <p>É com grande alegria que damos boas-vindas à nossa equipe! Estamos empolgados por tê-lo(a) conosco nessa jornada de saúde e bem-estar.</p>

            <p>Para acessar o sistema, utilize a senha gerada abaixo:</p>

            <div class='senha-box'>
                Sua senha: <span>{senha}</span>
            </div>

            <p>Recomendamos que você altere essa senha assim que fizer login.</p>

            <p>Seja bem-vindo(a) e conte conosco para o que precisar!</p>
        </div>
        <div class='logo'>
            <img src='{App.Logo}' alt='Logo do sistema' />
        </div>
    </div>
</body>
</html>
";


        await _emailService.EnviarEmailAsync(model.Settings, model.EmailDoDestinatario, assunto, corpo);
    }
}
