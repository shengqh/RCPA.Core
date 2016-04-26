using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    public AnnotationFormat(string ignoreHeaderPattern = null)
    {
      this.ignoreHeaderPattern = ignoreHeaderPattern;
      this.EndRegex = null;
    }

    public Regex EndRegex { get; set; }

    public virtual List<Annotation> ReadFromFile(string fileName)
    {
      var result = new List<Annotation>();

      using (StreamReader sr = new StreamReader(fileName))
      {
        string line;
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

        while ((line = sr.ReadLine()) != null)
        {
          if (line.Trim() == string.Empty)
          {
            break;
          }

          if(EndRegex != null && EndRegex.Match(line).Success)
          {
            break;
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
