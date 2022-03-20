using System;
using System.Collections.Generic;
using System.Linq;

namespace RCPA
{
  public static class EnumUtils
  {
    public static T[] EnumToArray<T>()
    {
      return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
    }

    public static object[] EnumToObjectArray<T>()
    {
      return Enum.GetValues(typeof(T)).Cast<object>().ToArray();
    }

    public static string[] EnumToStringArray<T>()
    {
      return Enum.GetValues(typeof(T)).Cast<T>().ToList().ConvertAll(m => m.ToString()).ToArray();
    }

    private static Dictionary<Type, Dictionary<string, object>> enumMap = new Dictionary<Type, Dictionary<string, object>>();

    public static bool EnumHasValue<T>(string name)
    {
      return EnumToStringArray<T>().Contains(name);
    }

    public static T StringToEnum<T>(string name, T defaultValue)
    {
      var type = defaultValue.GetType();

      if (!enumMap.ContainsKey(type))
      {
        enumMap[type] = EnumToObjectArray<T>().ToDictionary(m => m.ToString());
      }

      var items = enumMap[type];
      if (items.ContainsKey(name))
      {
        return (T)items[name];
      }
      else
      {
        return defaultValue;
      }
    }
  }
}
