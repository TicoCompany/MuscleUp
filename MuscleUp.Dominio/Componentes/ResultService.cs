namespace MuscleUp.Dominio.Componentes;

public class ResultService<T>
{
    public bool Sucesso { get; init; }
    public string? Mensagem { get; init; }
    public T? Dados { get; init; }

    public static ResultService<T> Ok(T dados, string? mensagem = null) =>
        new ResultService<T> { Sucesso = true, Dados = dados, Mensagem = mensagem };

    public static ResultService<T> Falha(string mensagem) =>
        new ResultService<T> { Sucesso = false, Mensagem = mensagem };
}