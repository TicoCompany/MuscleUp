using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MuscleUp.Dominio.Componentes;

public static class EnumExtensions
{
    public static string DisplayName(this Enum value)
    {
        var member = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault();

        if (member != null)
        {
            var displayAttr = member
                .GetCustomAttribute<DisplayAttribute>();

            if (displayAttr != null)
                return displayAttr.Name;
        }

        return value.ToString();
    }

    public static List<IdNome> ToEnum<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new IdNome
            {
                Id = Convert.ToInt32(e),
                Nome = e.DisplayName()
            })
            .OrderBy(x => x.Nome)
            .ToList();
    }

    public static List<IdEnumValue> ToEnumName<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new IdEnumValue
            {
                EnumValue = Convert.ToInt32(e),
                Nome = e.DisplayName()
            })
            .OrderBy(x => x.Nome)
            .ToList();
    }
}

public class IdNome
{
    public int Id { get; set; }
    public string Nome { get; set; }
}

public class IdEnumValue
{
    public int EnumValue { get; set; }
    public string Nome { get; set; }
}
