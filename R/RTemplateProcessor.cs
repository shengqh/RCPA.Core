using RCPA.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RCPA.R
{
  public class RTemplateProcessor : AbstractThreadProcessor
  {
    protected IRTemplateProcessorOptions options;

    public RTemplateProcessor(IRTemplateProcessorOptions options)
    {
      this.options = options;

      if (string.IsNullOrEmpty(options.RExecute))
      {
        options.RExecute = ExternalProgramConfig.GetExternalProgram("R");
      }

      if (string.IsNullOrEmpty(options.RExecute))
      {
        throw new Exception("Define R location first!");
      }
    }

    public override IEnumerable<string> Process()
    {
      var outputdir = Path.GetDirectoryName(options.OutputFile).Replace("\\", "/");
      var inputfile = options.InputFile.Replace("\\", "/");
      var outputfile = options.OutputFile.Replace("\\", "/");

      var targetrfile = options.OutputFile + ".r";
      using (var sw = new StreamWriter(targetrfile))
      {
        sw.WriteLine("outputdir<-\"{0}\"", outputdir);
        sw.WriteLine("inputfile<-\"{0}\"", inputfile);
        sw.WriteLine("outputfile<-\"{0}\"", outputfile);

        WriteAdditionalDefinitions(sw);

        string line = File.ReadAllText(options.RTemplate);
        using (var sr = new StreamReader(options.RTemplate))
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

      return new RProcessor(options.RExecute, targetrfile, options.OutputFile).Process();
    }

    protected virtual void WriteAdditionalDefinitions(StreamWriter sw) { }
  }
}
