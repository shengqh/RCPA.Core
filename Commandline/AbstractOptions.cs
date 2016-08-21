using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;
using System.Linq;

namespace RCPA.Commandline
{
  public abstract class AbstractOptions
  {
    private readonly List<string> _parsingErrors = new List<string>();

    protected AbstractOptions()
    {
      IsPileup = false;
    }

    public bool IsPileup { get; set; }

    public List<string> ParsingErrors
    {
      get { return _parsingErrors; }
    }

    [ParserState]
    public IParserState LastParserState { get; set; }

    protected void CheckFile(string key, string fileName)
    {
      if (!File.Exists(fileName))
      {
        ParsingErrors.Add(string.Format("File {0} not exists: {1}", key, fileName));
      }
    }

    protected void CheckDirectory(string key, string dirName)
    {
      if (!Directory.Exists(dirName))
      {
        ParsingErrors.Add(string.Format("Directory {0} not exists: {1}", key, dirName));
      }
    }

    protected bool CheckPattern(string pattern, string patternName, bool required=false)
    {
      if (string.IsNullOrWhiteSpace(pattern) && !required)
      {
        return true;
      }

      try
      {
        new Regex(pattern);
        return true;
      }
      catch (Exception)
      {
        _parsingErrors.Add(string.Format("Error regex pattern of {0} : {1}", patternName, pattern));
        return false;
      }
    }

    protected void CheckProperty(string propertyName)
    {
      var properties = this.GetType().GetProperties().ToDictionary(m => m.Name);
      if (!properties.ContainsKey(propertyName))
      {
        throw new ArgumentException("Property {0} not exist.", propertyName);
      }

      var pi = properties[propertyName];

      OptionAttribute oa = (OptionAttribute)Attribute.GetCustomAttribute(pi, typeof(OptionAttribute));
      if (oa == null)
      {
        throw new ArgumentException("Property {0} doesn't have attribute Option.", propertyName);
      }

      if (oa.MetaValue.Equals("FILE"))
      {
        string filename = pi.GetValue(this, null) as string;
        if (!string.IsNullOrEmpty(filename) && !File.Exists(filename))
        {
          ParsingErrors.Add(string.Format("File {0} not exists : {1}.", oa.HelpText, filename));
        }
      }
      else if (oa.MetaValue.Equals("DIRECTORY"))
      {
        string dirname = pi.GetValue(this, null) as string;
        if (!string.IsNullOrEmpty(dirname) && !Directory.Exists(dirname))
        {
          ParsingErrors.Add(string.Format("Directory {0} not exists : {1}.", oa.HelpText, dirname));
        }
      }else if (oa.MetaValue.Equals("FILELIST"))
      {
        IList<string> filenames = pi.GetValue(this, null) as IList<string>;
        foreach (var filename in filenames)
        {
          if (!string.IsNullOrEmpty(filename) && !File.Exists(filename))
          {
            ParsingErrors.Add(string.Format("File {0} not exists : {1}.", oa.HelpText, filename));
          }
        }
      }
    }

    [HelpOption]
    public string GetUsage()
    {
      HelpText result = HelpText.AutoBuild(this,
        (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

      if (_parsingErrors.Count > 0)
      {
        result.AddPreOptionsLine("ERROR(S):");
        foreach (string line in _parsingErrors)
        {
          result.AddPreOptionsLine("  " + line);
        }
      }

      return result;
    }

    public abstract bool PrepareOptions();

    public virtual void ResetDefaultValue(string[] args) { }
  }

  public abstract class AbstractFileOptions : AbstractOptions
  {
    public string OriginalFile { get; set; }
  }
}