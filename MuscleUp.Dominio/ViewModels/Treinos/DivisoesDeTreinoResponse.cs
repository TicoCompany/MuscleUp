namespace MuscleUp.Dominio.ViewModels.Treinos;

public sealed record DivisoesDeTreinoResponse
{
    public int IdTreino { get; set; } 
    public int DivisaoDeSubTreino { get; set; } 
    public string NomeDaDivisao { get; set; } 
    public List<object> Membros { get; set; } = new List<object>();  
}
