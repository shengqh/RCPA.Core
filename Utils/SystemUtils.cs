using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace RCPA.Utils
{
  public static class SystemUtils
  {
    public static void Execute(string executeFilename, string arguments)
    {
      ProcessStartInfo psi = new ProcessStartInfo();
      psi.FileName = executeFilename;
      psi.Arguments = arguments;
      psi.CreateNoWindow = false;
      psi.UseShellExecute = false;
      Process myProcess = System.Diagnostics.Process.Start(psi);
      myProcess.WaitForExit();
    }

    public static void Execute(string executeFilename, string arguments,string workingDirectory)
    {
      ProcessStartInfo psi = new ProcessStartInfo();
      psi.FileName = executeFilename;
      psi.Arguments = arguments;
      psi.CreateNoWindow = true;
      psi.UseShellExecute = false;
      psi.WorkingDirectory = workingDirectory;
      Process myProcess = System.Diagnostics.Process.Start(psi);
      myProcess.WaitForExit();
    }

    public static string CostMemory()
    {
      Process currentProcess = Process.GetCurrentProcess();
      return MyConvert.Format("{0:0.00} M", currentProcess.WorkingSet64 / (1024 * 1024));
    }

    public static bool IsLinux
    {
      get
      {
        int p = (int)Environment.OSVersion.Platform;
        return (p == 4) || (p == 6) || (p == 128);
      }
    }
  }
}
