﻿using System.ComponentModel.DataAnnotations.Schema;
using MuscleUp.Dominio.Exercicios;
using MuscleUp.Dominio.Treinos;
using MuscleUp.Dominio.Usuarios;

namespace MuscleUp.Dominio.Academias;

[Table("academias")]
public class Academia
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? Cnpj { get; set; }
    public string? Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Bairro { get; set; }
    public string? Estado { get; set; }
    public string? Cidade { get; set; }
    public string? Numero { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<Exercicio> Exercicios { get; set; } = new List<Exercicio>();
    public virtual ICollection<Treino> Treinos { get; set; } = new List<Treino>();

}
