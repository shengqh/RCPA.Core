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
      try
      {
        var options = CommandLine.Parser.Default.ParseArguments<T>(args,
          () => { result = false; }
          );

        if (result)
        {
          if (!options.PrepareOptions())
          {
            Console.Out.WriteLine(options.GetUsage());
            result = false;
          }
          else
          {
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
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(ex.StackTrace);
        result = false;
      }

      return result;
    }

    public abstract IProcessor GetProcessor(T options);

    #endregion
  }
}