using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RCPA.Parser;

namespace RCPA.Seq
{
  public static class AccessNumberParserFactory
  {
    public static readonly string SECTION_NAME = "DatabaseParseDefinitions";

    private static IAccessNumberParser autoParser;

    private static List<IAccessNumberParser> parsers;

    public static IAccessNumberParser GetAutoParser()
    {
      if (autoParser == null)
      {
        InitParsers();
      }

      return autoParser;
    }

    public static List<IAccessNumberParser> GetParsers()
    {
      if (parsers == null)
      {
        InitParsers();
      }

      return parsers;
    }

    private static void InitParsers()
    {
      ParserFormatList lstFormat = ParserFormatList.ReadFromOptionFile(SECTION_NAME);

      parsers = GetParsers(lstFormat);

      autoParser = parsers.Find(m => m.FormatName.Equals(AutoAccessNumberParser.FORMAT_NAME));
      if (autoParser == null)
      {
        autoParser = DefaultAccessNumberParser.GetInstance();
      }
    }

    public static List<IAccessNumberParser> GetParsers(ParserFormatList lstFormat)
    {
      List<IAccessNumberParser> result = new List<IAccessNumberParser>();

      if (null != lstFormat && lstFormat.Count > 0)
      {
        List<IAccessNumberParser> parsers = lstFormat.ConvertAll(m => (IAccessNumberParser)new AccessNumberParser(m));

        parsers.ForEach(p => result.Add(new NoExceptionAccessNumberParser(p)));

        result.Add(new AutoAccessNumberParser(parsers));
      }
      else
      {
        result.Add(DefaultAccessNumberParser.GetInstance());
      }

      return result;
    }

    public static IAccessNumberParser FindParserByName(string formatName)
    {
      string lowerFormatName = formatName.ToLower();

      return GetParsers().Find(m => m.FormatName.ToLower().Equals(lowerFormatName));
    }

    public static IAccessNumberParser FindParserByRegexPattern(string regexString)
    {
      return GetParsers().Find(m => m.RegexPattern.Equals(regexString));
    }

    public static IAccessNumberParser FindOrCreateParser(string regexString, string formatName)
    {
      IAccessNumberParser result = FindParserByRegexPattern(regexString);

      if (null == result)
      {
        result = FindParserByName(formatName);
      }

      if (null == result)
      {
        result = new NoExceptionAccessNumberParser(new AccessNumberParser(regexString, formatName));
      }

      return result;
    }

    public static IAccessNumberParser GuessParser(string databaseFileName)
    {
      FastaFormat ff = new FastaFormat();

      using (StreamReader sr = new StreamReader(databaseFileName))
      {
        Sequence seq;
        while ((seq = ff.ReadSequence(sr)) != null)
        {
          if (seq.Reference.StartsWith("IPI"))
          {
            return new NoExceptionAccessNumberParser(new IPIAccessNumberParser());
          }

          if (seq.Reference.StartsWith("gi|"))
          {
            return new NoExceptionAccessNumberParser(new NRAccessNumberParser());
          }
        }
      }

      return DefaultAccessNumberParser.GetInstance();
    }

  }
}
