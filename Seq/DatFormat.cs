using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RCPA.Seq
{
  public class DatFormat : ISequenceFormat
  {
    private Regex nameReg = new Regex(@"ID\s+(\S+)");
    private Regex recReg = new Regex(@"Full=(.+)");
    #region ISequenceFormat Members

    public Sequence ReadSequence(StreamReader reader)
    {
      string line;

      while ((line = reader.ReadLine()) != null)
      {
        if ((line.Length > 0) && (line.StartsWith("ID")))
        {
          break;
        }
      }

      if (reader.EndOfStream)
      {
        return null;
      }

      string name = nameReg.Match(line).Groups[1].Value;

      string description = "" ;
      while ((line = reader.ReadLine()) != null)
      {
        if (line.StartsWith("DE") && line.Contains("RecName:"))
        {
          description = recReg.Match(line).Groups[1].Value;
          break;
        }
      }

      while ((line = reader.ReadLine()) != null)
      {
        if (line.StartsWith("SQ"))
        {
          break;
        }
      }

      StringBuilder seq = new StringBuilder();
      while ((line = reader.ReadLine()) != null)
      {
        if (line.StartsWith("//"))
        {
          break;
        }
        seq.Append(line.Replace(" ", ""));
      }

      return new Sequence(name + " " + description, seq.ToString());
    }

    public void WriteSequence(StreamWriter writer, Sequence seq)
    {
    }

    public void WriteSequence(StreamWriter writer, string reference, string sequence)
    {
    }

    #endregion
  }
}