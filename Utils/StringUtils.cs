using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace System
{
  public static class StringUtils
  {
    public static string GetDoubleFormat(int decimalCount)
    {
      if (decimalCount <= 0)
      {
        return "{0:0}";
      }
      else
      {
        return "{0:0." + new string('0', decimalCount) + "}";
      }
    }

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

    public static string ToDoubleQuotes(this string source)
    {
      if (string.IsNullOrEmpty(source))
      {
        return "\"\"";
      }
      return "\"" + source + "\"";
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

    public static string ByteToHexString(byte[] data)
    {
      // Create a new Stringbuilder to collect the bytes 
      // and create a string.
      StringBuilder sBuilder = new StringBuilder();

      // Loop through each byte of the hashed data  
      // and format each one as a hexadecimal string. 
      for (int i = 0; i < data.Length; i++)
      {
        sBuilder.Append(data[i].ToString("x2"));
      }

      // Return the hexadecimal string. 
      return sBuilder.ToString();
    }

    public static string GetMd5Hash(MD5 md5Hash, string input)
    {
      // Convert the input string to a byte array and compute the hash. 
      return ByteToHexString(md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }
  }
}
