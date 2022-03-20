using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RCPA.Parser
{
  public class ParserFormatList : List<ParserFormat>
  {
    public static ParserFormatList ReadFromOptionFile(string sectionName)
    {
      string optionFile = AppDomain.CurrentDomain.BaseDirectory + "/MiscOptions.xml";
      if (File.Exists(optionFile))
      {
        var result = new ParserFormatList();

        result.ReadFromFile(optionFile, sectionName);

        result.Sort((p1, p2) => p1.GUInameIndex - p2.GUInameIndex);

        return result;
      }
      else
      {
        bool designMode = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        if (!designMode)
        {
          Console.Error.WriteLine(string.Format("Cannot find option file {0}", optionFile));
          throw new Exception(string.Format("Cannot find option file {0}", optionFile));
        }
        else
        {
          return new ParserFormatList();
        }
      }
    }

    public void ReadFromFile(string fileName, string sectionName)
    {
      Clear();

      if (!File.Exists(fileName))
      {
        throw new FileNotFoundException("File not found", fileName);
      }

      XElement doc = XElement.Load(fileName);

      var sectionXml = doc.Element(sectionName);

      if (null == sectionXml)
      {
        throw new ArgumentException(MyConvert.Format("Section {0} not exists in file {1}", sectionName, fileName), "sectionName");
      }

      var formatXmls =
        from format in sectionXml.Descendants("parseFormat")
        orderby Convert.ToInt32(format.Element("GUInameIndex").Value)
        select format;

      foreach (var formatXml in formatXmls)
      {
        var formatItem = new ParserFormat();

        formatItem.FormatName = formatXml.Element("formatName").Value;
        formatItem.FormatId = formatXml.Element("formatID").Value;
        formatItem.GUInameIndex = Convert.ToInt32(formatXml.Element("GUInameIndex").Value);

        var sample = formatXml.Element("formatSample");
        if (null != sample)
        {
          formatItem.Sample = sample.Value;
        }

        var reg = formatXml.Element("regex");
        if (null != reg)
        {
          formatItem.IdentityRegex = reg.Value;
        }

        var parseItemXmls = formatXml.Descendants("parseItem");
        foreach (var parseItemXml in parseItemXmls)
        {
          var parseItem = new ParserItem();

          parseItem.ItemName = parseItemXml.Element("itemName").Value;
          parseItem.RegularExpression = parseItemXml.Element("regularExpression").Value;
          parseItem.Slope = MyConvert.ToDouble(parseItemXml.Element("slope").Value);
          parseItem.Offset = MyConvert.ToDouble(parseItemXml.Element("offset").Value);

          formatItem.Add(parseItem);
        }

        Add(formatItem);
      }
    }
  }
}