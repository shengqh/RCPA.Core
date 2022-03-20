using System;
using System.IO;
using System.Text;

namespace RCPA.Seq
{
  public class FastaFormat : ISequenceFormat
  {
    private int widthPerLine;
    public FastaFormat(int widthPerLine = 70)
    {
      this.widthPerLine = widthPerLine;
    }

    #region ISequenceFormat Members

    public Sequence ReadSequence(StreamReader reader)
    {
      string line;

      while ((line = reader.ReadLine()) != null)
      {
        if ((line.Length > 0) && ('>' == line[0]))
        {
          break;
        }
      }

      if (reader.EndOfStream)
      {
        return null;
      }

      string information = line.Substring(1, line.Length - 1);

      var sb = new StringBuilder();
      while ((!reader.EndOfStream) && ('>' != reader.Peek()))
      {
        sb.Append(reader.ReadLine().Trim());
      }

      return new Sequence(information, sb.ToString());
    }

    public void WriteSequence(StreamWriter writer, Sequence seq)
    {
      WriteSequence(writer, seq.Reference, seq.SeqString);
    }

    public void WriteSequence(StreamWriter writer, string reference, string sequence)
    {
      writer.WriteLine(">" + reference);

      int pos = 0;
      while (pos < sequence.Length)
      {
        int count = Math.Min(widthPerLine, sequence.Length - pos);
        writer.WriteLine(sequence.Substring(pos, count));
        pos += count;
      }
    }

    #endregion
  }
}