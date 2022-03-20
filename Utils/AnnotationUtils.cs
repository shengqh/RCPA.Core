using System;
using System.Collections.Generic;
using System.Linq;

namespace RCPA
{
  public static class AnnotationUtils
  {
    public static readonly string ENABLED_KEY = "Enabled";

    public static List<string> GetAnnotationKeys<T>(IEnumerable<T> mps) where T : IAnnotation
    {
      var keyQuery =
        from ann in mps
        from key in ann.Annotations.Keys
        from k in key.Split('\t')
        orderby k
        select k;

      return keyQuery.Distinct().ToList();
    }

    public static string GetAnnotationHeader<T>(IEnumerable<T> mps) where T : IAnnotation
    {
      List<string> keys = GetAnnotationKeys(mps);

      return StringUtils.Merge(keys, "\t");
    }

    public static bool IsEnabled(this IAnnotation ann, bool defaultValue)
    {
      if (!ann.Annotations.ContainsKey(ENABLED_KEY))
      {
        return defaultValue;
      }

      bool result;
      if (!bool.TryParse(ann.Annotations[ENABLED_KEY].ToString(), out result))
      {
        return defaultValue;
      }
      else
      {
        return result;
      }
    }

    public static void SetEnabled(this IAnnotation ann, bool value)
    {
      ann.Annotations[ENABLED_KEY] = value.ToString();
    }
  }
}