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
}
