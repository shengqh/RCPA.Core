using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RCPA.Seq
{
  public class Sequence
  {
    private static Regex reg = new Regex(@"\s*(\S+)\s*(.*)");

    public string SeqString { get; set; }

    private string name;

    public string Name
    {
      get { return name; }
    }

    private string reference;

    public string Reference
    {
      get { return reference; }
      set
      {
        SetReference(value);
      }
    }

    private string description;

    public string Description
    {
      get { return description; }
    }

    public Sequence(string reference, string seqString)
    {
      this.SeqString = seqString;
      SetReference(reference);
    }

    private Dictionary<string, Object> annotation;

    public Dictionary<string, Object> Annotation
    {
      get
      {
        if (null == annotation)
        {
          annotation = new Dictionary<string, object>();
        }
        return annotation;
      }
    }

    private void SetReference(string reference)
    {
      this.reference = reference;
      var m = reg.Match(reference);
      if (m.Success)
      {
        this.name = m.Groups[1].Value;
        this.description = m.Groups[2].Value;
      }
      else
      {
        this.name = reference;
        this.description = "";
      }
    }

    public override string ToString()
    {
      return this.name;
    }
  }
}
