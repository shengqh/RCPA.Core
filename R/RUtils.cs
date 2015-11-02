using RCPA.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCPA.R
{
  public static class RUtils
  {
    public static void PerformRByTemplate(string rfile, string targetrfile, List<string> definitions)
    {
      using (var sw = new StreamWriter(targetrfile))
      {
        sw.NewLine = "\n";
        foreach (var def in definitions)
        {
          sw.WriteLine(def);
        }
        string line = File.ReadAllText(rfile);
        using (var sr = new StreamReader(rfile))
        {
          if (line.Contains("#predefine_end"))
          {
            while ((line = sr.ReadLine()) != null)
            {
              if (line.Contains("#predefine_end"))
              {
                break;
              }
            }
          }

          while ((line = sr.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
      }
      SystemUtils.Execute("R", "--vanilla --slave -f \"" + targetrfile + "\"");
    }
  }
}
