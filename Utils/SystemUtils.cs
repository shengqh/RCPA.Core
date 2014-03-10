using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace RCPA.Utils
{
  public static class SystemUtils
  {
    private static object GetRegisteryValueRInstallPath()
    {
      var v = RegistryHelpers.GetRegistryValue(@"SOFTWARE\R-core\R\", "InstallPath");
      return v;
    }

    public static string GetRscriptExecuteLocation()
    {
      var v = GetRegisteryValueRInstallPath();
      if (v == null)
      {
        return null;
      }
      return (string)v + (Environment.Is64BitOperatingSystem ? @"\bin\x64\Rscript.exe" : @"\bin\Rscript.exe");
    }

    public static string GetRExecuteLocation()
    {
      var v = GetRegisteryValueRInstallPath();
      if (v == null)
      {
        return null;
      }
      return (string)v + (Environment.Is64BitOperatingSystem ? @"\bin\x64\R.exe" : @"\bin\R.exe");
    }

    public static void Execute(string executeFilename, string arguments)
    {
      Console.WriteLine("Processing {0} {1}", executeFilename, arguments);
      ProcessStartInfo psi = new ProcessStartInfo();
      psi.FileName = executeFilename;
      psi.Arguments = arguments;
      psi.CreateNoWindow = false;
      psi.UseShellExecute = false;
      Process myProcess = System.Diagnostics.Process.Start(psi);
      myProcess.WaitForExit();
    }

    public static void Execute(string executeFilename, string arguments, string workingDirectory)
    {
      Console.WriteLine("Processing {0} {1}", executeFilename, arguments);
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
