using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace RCPA.Utils
{
  public static class MiscUtils
  {
    public static List<string> ToList(this Match matcher)
    {
      List<String> result = new List<String>();
      for (int i = 1; i < matcher.Groups.Count; i++)
      {
        result.Add(matcher.Groups[i].Value);
      }
      return result;
    }
  }
}
