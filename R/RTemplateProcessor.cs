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

    private static readonly string PREDEFINED_END = "#predefine_end";

    public override IEnumerable<string> Process()
    {
      if (!File.Exists(options.RTemplate))
      {
        throw new FileNotFoundException("Cannot find R template " + options.RTemplate);
      }

      var outputdir = Path.GetDirectoryName(options.OutputFile).Replace("\\", "/");
      var inputfile = options.InputFile.Replace("\\", "/");

      var targetrfile = options.OutputFile + ".r";
      Progress.SetMessage("Saving r file to " + targetrfile + "...");
      using (var sw = new StreamWriter(targetrfile))
      {
        sw.WriteLine("outputdir<-\"{0}\"", outputdir);
        sw.WriteLine("inputfile<-\"{0}\"", inputfile);
        if (!options.NoResultFile)
        {
          var outputfile = options.OutputFile.Replace("\\", "/");
          sw.WriteLine("outputfile<-\"{0}\"", outputfile);
        }

        Progress.SetMessage("Writing parameters ...");
        foreach (var param in options.Parameters)
        {
          sw.WriteLine(param);
        }

        Progress.SetMessage("Writing additional informations ...");
        WriteAdditionalDefinitions(sw);

        bool hasPredefined = File.ReadAllText(options.RTemplate).Contains(PREDEFINED_END);
        string line;
        using (var sr = new StreamReader(options.RTemplate))
        {
          if (hasPredefined)
          {
            Progress.SetMessage("Ignore predefined section ...");
            while ((line = sr.ReadLine()) != null)
            {
              if (line.Contains(PREDEFINED_END))
              {
                break;
              }
            }
          }

          Progress.SetMessage("Copy lines from template " + options.RTemplate + "...");
          while ((line = sr.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
      }
      Progress.SetMessage("R file saved to " + targetrfile);

      Progress.SetMessage("Processing R file " + targetrfile + "...");

      var result = new RProcessor(new RProcessorOptions()
      {
        RExecute = options.RExecute,
        RFile = targetrfile,
        ExpectResultFile = options.NoResultFile ? string.Empty : options.OutputFile,
        CreateNoWindow = options.CreateNoWindow
      })
      { Progress = this.Progress }.Process();

      Progress.SetMessage("R file " + targetrfile + " processed.");

      return result;
    }

    protected virtual void WriteAdditionalDefinitions(StreamWriter sw) { }
  }
}
