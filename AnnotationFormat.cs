using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RCPA
{
  public class AnnotationFormat : IFileFormat<List<Annotation>>
  {
    private LineFormat<Annotation> _format { get; set; }

    public LineFormat<Annotation> Format
    {
      get
      {
        return _format;
      }
      set
      {
        _format = value;
      }
    }

    private string ignoreHeaderPattern;
    private bool hasHeader;
    public AnnotationFormat(string ignoreHeaderPattern = null, bool hasHeader = true)
    {
      this.ignoreHeaderPattern = ignoreHeaderPattern;
      this.hasHeader = hasHeader;
      this.EndRegex = null;
    }

    public Regex EndRegex { get; set; }

    public virtual List<Annotation> ReadFromFile(string fileName)
    {
      var result = new List<Annotation>();

      using (StreamReader sr = new StreamReader(fileName))
      {
        string line;

        if (hasHeader)
        {
          while ((line = sr.ReadLine()) != null)
          {
            if (string.IsNullOrEmpty(ignoreHeaderPattern))
            {
              break;
            }

            if (!Regex.Match(line, ignoreHeaderPattern).Success)
            {
              break;
            }
          }

          InitializeLineFormat(line);
        }
        else
        {
          _format = null;
        }

        while ((line = sr.ReadLine()) != null)
        {
          if (line.Trim() == string.Empty)
          {
            break;
          }

          if (EndRegex != null && EndRegex.Match(line).Success)
          {
            break;
          }

          if (!hasHeader && _format == null)
          {
            var parts = line.Split('\t');
            var header = (from i in Enumerable.Range(1, parts.Length) select "X" + i.ToString()).Merge("\t");
            InitializeLineFormat(header);
          }

          result.Add(Format.ParseString(line));
        }
      }

      return result;
    }


    public void InitializeLineFormat(string header)
    {
      _format = new LineFormat<Annotation>(AnnotationPropertyFactory.GetInstance(), header);
    }

    public void InitializeLineFormat(List<Annotation> t)
    {
      var anns = (from ann in t
                  from key in ann.Annotations.Keys
                  select key).Distinct().OrderBy(m => m).Merge('\t');

      _format = new LineFormat<Annotation>(AnnotationPropertyFactory.GetInstance(), anns);
    }

    public virtual void WriteToFile(string fileName, List<Annotation> t)
    {
      if (_format == null)
      {
        InitializeLineFormat(t);
      }

      using (StreamWriter sw = new StreamWriter(fileName))
      {
        sw.WriteLine(Format.Headers);
        t.ForEach(m => sw.WriteLine(Format.GetString(m)));
      }
    }
  }
}
