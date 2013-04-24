using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace System
{
  public static class StringUtils
  {
    public static void AddTo(StringBuilder sb, IEnumerable<string> source, object delimiter)
    {
      bool first = true;
      foreach (string current in source)
      {
        if (first)
        {
          sb.Append(current);
          first = false;
        }
        else
        {
          sb.Append(delimiter);
          sb.Append(current);
        }
      }
    }

    public static string Merge(this IEnumerable<string> source, object delimiter)
    {
      StringBuilder sb = new StringBuilder();
      AddTo(sb, source, delimiter);
      return sb.ToString();
    }

    public static string LeftFill(object obj, int length, char fillChar)
    {
      StringBuilder sb = new StringBuilder(obj.ToString());
      while (sb.Length < length)
      {
        sb.Insert(0, fillChar);
      }
      return sb.ToString();
    }

    public static string GetMergedHeader(string header, IEnumerable<string> annotations, char delimiter)
    {
      string str = header + delimiter + Merge(annotations, delimiter);

      var keys = str.Split(delimiter).Distinct();

      return Merge(keys, delimiter);
    }

    public static string RepeatChar(char p, int count)
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < count; i++)
      {
        sb.Append(p);
      }
      return sb.ToString();
    }

    public static string StringBefore(this string source, string delimiter)
    {
      var index = source.IndexOf(delimiter);
      if (index >= 0)
      {
        return source.Substring(0, index);
      }
      else
      {
        return source;
      }
    }

    public static string StringAfter(this string source, string delimiter)
    {
      var index = source.IndexOf(delimiter);
      if (index >= 0)
      {
        return source.Substring(index + delimiter.Length);
      }
      else
      {
        return source;
      }
    }
  }
}
