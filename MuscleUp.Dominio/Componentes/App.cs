namespace MuscleUp.Dominio.Componentes;

public static class App
{
    public const string Nome = "MuscleUp";
    public const string Email = "ticocompany3@gmail.com";
    public const string ChaveSecreta = "phhf420b-76c1-4026-b0a1-9bd1ed1736e6-711f2a0c-59a7-10pf-acc5-be0bbe5da6d1";
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
