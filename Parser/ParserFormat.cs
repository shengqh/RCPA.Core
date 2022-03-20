using System.Collections.Generic;

namespace RCPA.Parser
{
  public class ParserFormat : List<ParserItem>
  {
    public string FormatName { get; set; }

    public string FormatId { get; set; }

    public int GUInameIndex { get; set; }

    public string Sample { get; set; }

    public string IdentityRegex { get; set; }

    public override string ToString()
    {
      if (Sample != null)
      {
        return FormatName + " --- example : " + Sample;
      }
      else
      {
        return FormatName;
      }
    }
  }
}