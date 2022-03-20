using System.Collections.Generic;
using System.IO;

namespace RCPA.Seq
{
  public class DatabaseUtils
  {
    public static Dictionary<string, Sequence> GetAccessNumberMap(string database)
    {
      return GetAccessNumberMap(database, AccessNumberParserFactory.GuessParser(database));
    }

    public static Dictionary<string, Sequence> GetAccessNumberMap(string database, IStringParser<string> acParser)
    {
      Dictionary<string, Sequence> result = new Dictionary<string, Sequence>();

      using (StreamReader sr = new StreamReader(database))
      {
        FastaFormat sf = new FastaFormat();
        Sequence seq;
        while ((seq = sf.ReadSequence(sr)) != null)
        {
          string ac = acParser.GetValue(seq.Name);
          result[ac] = seq;
        }
      }

      return result;
    }
  }
}
