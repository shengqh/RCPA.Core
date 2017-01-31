using System;
using System.Linq;
using CommandLine;
using RCPA.Commandline;
using RCPA;
using System.IO;

namespace RCPA.Commandline
{
  public interface ICommandLineCommand
  {
    string Name { get; }

    string Description { get; }

    bool Process(string[] args);
  }

  public abstract class AbstractCommandLineCommand<T> : ICommandLineCommand where T : AbstractOptions, new()
  {
    #region ICommandLineCommand Members

    public abstract string Name { get; }

    public abstract string Description { get; }

    public virtual bool Process(string[] args)
    {
      var result = true;
      if (System.Diagnostics.Debugger.IsAttached)
      {
        result = DoProcess(args, result);
      }
      else
      {
        try
        {
          result = DoProcess(args, result);
        }
        catch (Exception ex)
        {
          Console.Error.WriteLine("Error: " + ex.Message);
          Console.Error.WriteLine("Trace: " + ex.StackTrace);
          result = false;
        }
      }

      return result;
    }

    private bool DoProcess(string[] args, bool result)
    {
      var options = new T();
      result = CommandLine.Parser.Default.ParseArguments(args, options);
      if (result)
      {
        if (!options.PrepareOptions())
        {
          Console.Out.WriteLine(options.GetUsage());
          result = false;
        }
        else
        {
          options.ResetDefaultValue(args);

          var files = GetProcessor(options).Process();
          if (files != null && files.Count() > 0)
          {
            if (files.All(File.Exists))
            {
              Console.WriteLine("File saved to :\n" + files.Merge("\n"));
            }
            else
            {
              Console.WriteLine(files.Merge("\n"));
            }
          }
        }
      }
      else
      {
        Console.Out.WriteLine((from er in options.LastParserState.Errors select er.ToString()).Merge("\n"));
        result = false;
      }
      return result;
    }

    public abstract IProcessor GetProcessor(T options);

    #endregion
  }
}