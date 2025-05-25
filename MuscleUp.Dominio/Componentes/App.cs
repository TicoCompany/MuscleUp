namespace MuscleUp.Dominio.Componentes;

public static class App
{
    public const string Nome = "MuscleUp";
    public const string Email = "ticocompany3@gmail.com";
    public static readonly string Url = !EstaEmDebug ? "http://muscleup.com" : "http://localhost:5080";
    public static readonly string Logo = !EstaEmDebug ? "http://muscleup.com/images/logo.jpeg" : "https://img.freepik.com/vetores-premium/homem-mais-forte-design-de-logo-de-fitness_586862-615.jpg?w=2000";
    //public static readonly string Logo = !EstaEmDebug ? "http://muscleup.com/images/logo.jpeg" : "http://localhost:5080/images/logo.jpeg";


    public static bool EstaEmDebug =>
#if DEBUG
    true;
#else
        false;
#endif
}
