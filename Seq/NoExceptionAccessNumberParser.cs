using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA.Seq
{
  /// <summary>
  /// AccessNumber解析器。
  /// 当指定parser不起作用时，调用DefaultAccessNumberParser进行解析，不抛出异常
  /// </summary>
  public class NoExceptionAccessNumberParser : IAccessNumberParser
  {
    private IAccessNumberParser parser;

    public NoExceptionAccessNumberParser(IAccessNumberParser parser)
    {
      this.parser = parser;
    }

    #region IAccessNumberParser Members

    public string FormatName
    {
      get { return parser.FormatName; }
    }

    public string GetValue(string obj)
    {
      string result;

      if (!parser.TryParse(obj, out result))
      {
        result = DefaultAccessNumberParser.GetInstance().GetValue(obj);
      }

      return result;
    }

    public bool TryParse(string obj, out string value)
    {
      value = GetValue(obj);
      return true;
    }

    #endregion

    public override string ToString()
    {
      return parser.ToString();
    }

    #region IAccessNumberParser Members


    public string RegexPattern
    {
      get { return parser.RegexPattern; }
    }

    #endregion
  }

}
