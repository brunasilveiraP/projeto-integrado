using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BBL.Enums;

public static class EnumTools
{
    public static IDictionary<int, string> EnumToDictionary<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>().ToDictionary(
                t => Convert.ToInt32(t),
                t => t.GetAttributeOfType<DescriptionAttribute>().Description);
    }

    public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
}