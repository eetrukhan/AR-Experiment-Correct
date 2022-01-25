using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public class EnumDescription
{
    public static string getDescription(Enum enumElement)
    {
        Type type = enumElement.GetType();

        MemberInfo[] memInfo = type.GetMember(enumElement.ToString());
        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
        }

        return enumElement.ToString();
    }

    public static Color getColor(string name)
    {
        var colorProps = typeof(Color).GetProperties();
        foreach (var color in colorProps)
            if (name.Equals(color.Name, StringComparison.OrdinalIgnoreCase))
                return (Color)color.GetValue(new Color(), null);
        return Color.white;
    }
}
