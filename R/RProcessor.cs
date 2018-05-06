using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace RCPA.R
{
  public class RProcessorOptions
  {
    public RProcessorOptions()
    {
      CreateNoWindow = true;
    }

    public string RExecute { get; set; }
    public bool IsR
    {
      get
      {
        var rname = new FileInfo(RExecute).Name.ToLower();
        return rname.Equals("r.exe") || rname.Equals("r");
      }
    }

    public string RFile { get; set; }

    public string ExpectResultFile { get; set; }

    public bool CreateNoWindow { get; set; }
  }

  public class RProcessor : AbstractThreadProcessor
  {
    private RProcessorOptions options;

    public RProcessor(RProcessorOptions options)
    {
      this.options = options;
    }

    public override IEnumerable<string> Process()
    {
      var rfile = options.RFile.ToDoubleQuotes();
      var rproc = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = File.Exists(options.RExecute) ? options.RExecute.ToDoubleQuotes() : options.RExecute,
          Arguments = string.Format("--vanilla {0} ", options.IsR ? "-f " + rfile + " --slave" : rfile),
          UseShellExecute = false,
          RedirectStandardOutput = true,
          RedirectStandardError = true,
          CreateNoWindow = options.CreateNoWindow
        }
      };

      try
      {
        if (!rproc.Start())
        {
          throw new Exception("R command cannot be started, check your parameters and ensure that R is available.");
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("R command start failed : {0}", ex.Message));
      }

      var log = options.RFile + ".log";
      try
      {
        using (var sw = new StreamWriter(log))
        {
          string line;
          while ((line = rproc.StandardOutput.ReadLine()) != null)
          {
            sw.WriteLine(line);
            Progress.SetMessage(line);
          }

          while ((line = rproc.StandardError.ReadLine()) != null)
          {
            if (line.StartsWith("The system cannot find the path specified"))
            {
              if (Environment.Is64BitOperatingSystem)
              {
                throw new Exception("make sure you setup correct X64 version of the R at Setup->Extenal program!");
              }
              else
              {
                throw new Exception("make sure you setup correct X86 version of the R at Setup->Extenal program!");
              }
            }
            Progress.SetMessage("E:" + line);
            sw.WriteLine("E:" + line);
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("R command error : {0}", ex.Message));
      }

      if (!string.IsNullOrWhiteSpace(options.ExpectResultFile))
      {
        if (!File.Exists(options.ExpectResultFile))
        {
          if (File.Exists(log) && File.ReadAllText(log).Contains("nnls"))
          {
            throw new Exception("R command failed to genearte result. Make sure you have installed all necessary packages in R");
          }

          throw new Exception(string.Format("R command failed to genearte result as {0}. Check the log file {1}.\nYou can manully run the R script file {2} to find out the problem.", options.ExpectResultFile, log, options.RFile));
        }

        return new string[] { options.ExpectResultFile };
      }
      else
      {
        return null;
      }
    }
  }
}
