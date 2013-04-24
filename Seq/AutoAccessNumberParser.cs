using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA.Seq
{
  public class AutoAccessNumberParser : IAccessNumberParser
  {
    public static readonly string FORMAT_NAME = "AUTO";

    private List<IAccessNumberParser> parsers;

    public AutoAccessNumberParser(List<IAccessNumberParser> parsers)
    {
      this.parsers = new List<IAccessNumberParser>(parsers);
    }

    #region IAccessNumberParser Members

    public string FormatName
    {
      get { return FORMAT_NAME; }
    }

    public string GetValue(string name)
    {
      foreach (IAccessNumberParser parser in parsers)
      {
        string result;

        if (parser.TryParse(name, out result))
        {
          return result;
        }
      }

      return DefaultAccessNumberParser.GetInstance().GetValue(name);
    }

    public bool TryParse(string obj, out string value)
    {
      value = GetValue(obj);
      return true;
    }

    public string RegexPattern
    {
      get { return FORMAT_NAME; }
    }

    #endregion

    public override string ToString()
    {
      return FormatName;
    }
  }
}
