using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Gets the EnumMember value from an enum value.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>The EnumMember value if present; otherwise, null.</returns>
    public static string? GetEnumMemberValue<T>(this T enumValue) where T : Enum
    {
        var type = enumValue.GetType();
        var memInfo = type.GetMember(enumValue.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
        return attributes.Length > 0 ? ((EnumMemberAttribute)attributes[0]).Value : null;
    }

    /// <summary>
    /// Parses the EnumMember value to its corresponding enum value.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="enumMemberValue">The EnumMember value.</param>
    /// <returns>The corresponding enum value.</returns>
    public static T ParseFromEnumMemberValue<T>(this string enumMemberValue) where T : Enum
    {
        var type = typeof(T);
        return ParseFromEnumMemberValue<T>(enumMemberValue, () => throw new ArgumentException($"No EnumMember with value '{enumMemberValue}' found in enum '{type.Name}'.", nameof(enumMemberValue)));
    }

    public static T ParseFromEnumMemberValue<T>(this string enumMemberValue, T defaultValue) where T : Enum
    {
        return ParseFromEnumMemberValue<T>(enumMemberValue, () => defaultValue);
    }

    private static T ParseFromEnumMemberValue<T>(string enumMemberValue, Func<T> defaultValueResolver) where T : Enum
    {
        var type = typeof(T);
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attributes = field.GetCustomAttributes(typeof(EnumMemberAttribute), false);
            if (attributes.Length > 0 && ((EnumMemberAttribute)attributes[0]).Value == enumMemberValue)
            {
                return (T)field.GetValue(null);
            }
        }

        return defaultValueResolver();
    }
}