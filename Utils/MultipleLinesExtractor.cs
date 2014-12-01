using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RCPA.Utils
{
  public class MultipleLinesExtractor : AbstractThreadProcessor
  {
    private MultipleLinesExtractorOptions _options;

    public MultipleLinesExtractor(MultipleLinesExtractorOptions options)
    {
      this._options = options;
    }

    public override IEnumerable<string> Process()
    {
      using (var sw = new StreamWriter(_options.TargetFile))
      {
        using (var sr = new StreamReader(_options.SourceFile))
        {
          string line;
          int step = 0;
          while (((line = sr.ReadLine()) != null) && (step < _options.LineCount))
          {
            sw.WriteLine(line);
            step++;
          }
        }
      }

      return new string[] { _options.TargetFile };
    }
  }
}
