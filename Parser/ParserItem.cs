using System;
namespace RCPA.Parser
{
  public class ParserItem
  {
    public ParserItem()
    {
      Slope = 1;
      Offset = 0;
    }

    public ParserItem(string name, string regularExpression)
      : this()
    {
      ItemName = name;
      RegularExpression = regularExpression;
    }

    public string ItemName { get; set; }

    public string RegularExpression { get; set; }

    public double Slope { get; set; }

    public double Offset { get; set; }
  }
}