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

    public static string GetTimeCost(TimeSpan cost)
    {
      var result = new StringBuilder();

      if(cost.Days > 0)
      {
        result.Append(string.Format(" {0} days", cost.Days));
      }

      if (cost.Hours > 0)
      {
        result.Append(string.Format(" {0} hours", cost.Hours));
      }

      if (cost.Minutes > 0)
      {
        result.Append(string.Format(" {0} minutes", cost.Minutes));
      }

      if (cost.Seconds > 0)
      {
        result.Append(string.Format(" {0} Seconds", cost.Seconds));
      }

      return result.ToString();
    }
  }
}
