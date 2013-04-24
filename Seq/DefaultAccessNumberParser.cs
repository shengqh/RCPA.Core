using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RCPA.Seq
{
  public class DefaultAccessNumberParser : IAccessNumberParser
  {
    private static DefaultAccessNumberParser parser;

    public static DefaultAccessNumberParser GetInstance()
    {
      if (parser == null)
      {
        parser = new DefaultAccessNumberParser();
      }
      return parser;
    }

    private DefaultAccessNumberParser() { }

//    private Regex defaultRegex = new Regex(@"(\S{1,20})");
    private Regex defaultRegex = new Regex(@"(\S{1,30})");

    #region IAccessNumberParser Members

    public string FormatName
    {
      get { return "DEFAULT"; }
    }

    public string GetValue(string name)
    {
      Match m = defaultRegex.Match(name);

      if (m.Success)
      {
        return m.Groups[1].Value;
      }

      return name;
    }

    public bool TryParse(string obj, out string value)
    {
      value = GetValue(obj);
      return true;
    }

    #endregion

    #region IAccessNumberParser Members


    public string RegexPattern
    {
      get { return defaultRegex.ToString(); }
    }

    #endregion
  }
}
