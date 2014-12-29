using System;
using System.Collections.Generic;

namespace RCPA
{
  public interface IAnnotation
  {
    Dictionary<string, object> Annotations { get; }
  }

  public static class AnnotationExtension
  {
    public static double GetDoubleValue(this IAnnotation si, string key)
    {
      if (!si.Annotations.ContainsKey(key))
      {
        throw new Exception("There is no information of " + key);
      }
      return MyConvert.ToDouble(si.Annotations[key]);
    }

    public static bool HasDoubleValue(this IAnnotation si, string key)
    {
      if (!si.Annotations.ContainsKey(key))
      {
        return false;
      }

      double value;
      return MyConvert.TryParse(si.Annotations[key], out value);
    }
  }
}