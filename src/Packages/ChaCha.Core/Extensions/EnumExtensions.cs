using System.ComponentModel;

namespace ChaCha.Core.Extensions;

public static class EnumExtensions
{
  public static string GetDescription(this Enum value)
  {
    var field = value.GetType().GetField(value.ToString());
    var attribute = field?
      .GetCustomAttributes(typeof(DescriptionAttribute), false)
      .FirstOrDefault() as DescriptionAttribute;

    return attribute?.Description ?? value.ToString();
  }

  public static TEnum FromDescription<TEnum>(string description) where TEnum : struct, Enum
  {
    foreach (var field in typeof(TEnum).GetFields())
    {
      var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
      if (attribute != null && attribute.Description == description)
        return (TEnum)field.GetValue(null)!;
      if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
        return (TEnum)field.GetValue(null)!;
    }
    throw new ArgumentException($"No {typeof(TEnum).Name} with description '{description}' found.");
  }
}