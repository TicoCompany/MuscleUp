using System.Security.Cryptography;

namespace MuscleUp.Dominio.Componentes;

public static class GeradorSenha
{
    public static string GerarSenha(int comprimento = 8)
    {
        const string caracteresPermitidos = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var bytes = new byte[comprimento];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        var senha = new char[comprimento];
        for (int i = 0; i < comprimento; i++)
        {
            senha[i] = caracteresPermitidos[bytes[i] % caracteresPermitidos.Length];
        }

        return new string(senha);
    }
}
