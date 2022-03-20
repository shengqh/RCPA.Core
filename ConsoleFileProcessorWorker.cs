using RCPA.Gui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace RCPA
{
  public class ConsoleFileProcessorWorker : FileProcessorWorker
  {
    public ConsoleFileProcessorWorker(IFileProcessor fileProcessor, string originalFilename)
      : base(fileProcessor, originalFilename)
    {
      ProgressChanged += ConsoleProgressChanged;
      RunWorkerCompleted += ConsoleRunWorkerCompleted;
    }

    private void ConsoleRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled)
      {
        if (null != e.Result)
        {
          Console.Out.WriteLine("Task cancelled " + e.Result);
        }
        else
        {
          Console.Out.WriteLine("Task cancelled.");
        }
      }
      else if (e.Error != null)
      {
        Console.Error.WriteLine(e.Error.StackTrace);
        Console.Error.WriteLine("Error : " + e.Error.Message);
      }
      else
      {
        var resultFiles = (List<string>)e.Result;

        bool bIsFile = true;
        var sb = new StringBuilder();
        foreach (String file in resultFiles)
        {
          sb.Append("\n" + file);
          if (!new FileInfo(file).Exists)
          {
            bIsFile = false;
          }
        }

        if (0 == resultFiles.Count)
        {
          Console.Out.WriteLine("Task finished.");
        }
        else if (bIsFile)
        {
          Console.Out.WriteLine("Result has saved to : " + sb);
        }
        else
        {
          Console.Out.WriteLine(sb.ToString());
        }
      }
    }

    private void ConsoleProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (e.UserState is WorkerProgressUserState)
      {
        var eState = (WorkerProgressUserState)e.UserState;
        if (eState.IsProgress)
        {
          Console.Out.WriteLine(eState.ProgressValue + "%");
        }
        else
        {
          Console.Out.WriteLine(eState.LabelText);
        }
        return;
      }

      if (e.UserState is string)
      {
        Console.Out.WriteLine(e.UserState);
        return;
      }

      Console.Out.WriteLine(e.ProgressPercentage + "%");
    }
  }
}