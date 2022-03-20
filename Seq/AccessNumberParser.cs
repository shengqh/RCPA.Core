using RCPA.Parser;
using System;
using System.Text.RegularExpressions;

namespace RCPA.Seq
{
  /// <summary>
  /// AccessNumber解析器。
  /// 当指定pattern不起作用时，抛出ArgumentException
  /// </summary>
  public class AccessNumberParser : IAccessNumberParser
  {
    private string regexString;

    private string formatName;

    private string example;

    public AccessNumberParser(string regexString, string formatName)
    {
      this.formatName = formatName;

      this.regexString = regexString;

      this.example = null;

      InitRegex();
    }

    public AccessNumberParser(string regexString, string formatName, string example)
      : this(regexString, formatName)
    {
      this.example = example;
    }

    public AccessNumberParser(ParserFormat format)
    {
      formatName = format.FormatName;
      example = format.Sample;

      foreach (ParserItem item in format)
      {
        if (item.ItemName.Equals("accessNumber") && item.RegularExpression.Length > 0)
        {
          regexString = item.RegularExpression;
        }
      }

      if (regexString == null)
      {
        throw new ArgumentException("There is no accessNumber parsing definition at " + format.FormatName);
      }

      InitRegex();
    }

    private void InitRegex()
    {
      try
      {
        acRegex = new Regex(regexString);
      }
      catch (Exception ex)
      {
        throw new ArgumentException("Unvalid regex expression " + regexString, ex);
      }
    }

    private Regex acRegex;

    public Regex AccessNumberRegex { get { return acRegex; } }

    #region IParser<string> Members

    public string FormatName
    {
      get { return formatName; }
    }

    public string GetValue(string name)
    {
      Match matcher = acRegex.Match(name);

      if (matcher.Success)
      {
        if (1 == matcher.Groups.Count)
        {
          throw new InvalidOperationException("No captured access number in pattern " + RegexPattern);
        }

        return matcher.Groups[1].Value;
      }

      throw new Exception(name + " is not a valid " + formatName + " name!");
    }

    public bool TryParse(string name, out string value)
    {
      Match matcher = acRegex.Match(name);

      if (matcher.Success)
      {
        value = matcher.Groups[1].Value;
        return true;
      }

      value = name;
      return false;
    }

    public string RegexPattern
    {
      get { return regexString; }
    }

    #endregion

    public override string ToString()
    {
      if (this.example == null)
      {
        return formatName + " : " + regexString;
      }

      return formatName + " : " + regexString + " --- " + example;
    }
  }

  public class IPIAccessNumberParser : AccessNumberParser
  {
    public IPIAccessNumberParser() : base(@"IPI:([^.|]*)", "EBI-IPI") { }
  }

  public class NRAccessNumberParser : AccessNumberParser
  {
    public NRAccessNumberParser() : base(@"(gi.\d+)", "NCBI-NR") { }
  }

  public class SwissProtAccessNumberParser : AccessNumberParser
  {
    public SwissProtAccessNumberParser() : base(@"^>{0,1}(\S+)", "SwissProt") { }
  }
}
