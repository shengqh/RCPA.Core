using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Reflection;

namespace RCPA.Utils
{
  public enum SystemType { Windows, Mono3Lower, Mono4Upper };

  public static class SystemUtils
  {
    public static SystemType CurrentSystem { get; private set; }

    static SystemUtils()
    {
      CurrentSystem = SystemType.Windows;

      Type type = Type.GetType("Mono.Runtime");
      if (type != null)
      {
        CurrentSystem = SystemType.Mono4Upper;

        MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
        if (displayName != null)
        {
          var name = displayName.Invoke(null, null).ToString();
          //Console.WriteLine("Current mono version = {0}", name);
          if (name.Length > 1 && Char.IsDigit(name[0]) && int.Parse(name[0].ToString()) <= 3)
          {
            CurrentSystem = SystemType.Mono3Lower;
          }
        }
      }
    }

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
        return CurrentSystem != SystemType.Windows;
      }
    }

    [DllImport("kernel32.dll")]
    static extern bool AttachConsole(UInt32 dwProcessId);

    [DllImport("kernel32.dll")]
    private static extern bool GetFileInformationByHandle(SafeFileHandle hFile, out BY_HANDLE_FILE_INFORMATION lpFileInformation);

    [DllImport("kernel32.dll")]
    private static extern SafeFileHandle GetStdHandle(UInt32 nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool SetStdHandle(UInt32 nStdHandle, SafeFileHandle hHandle);

    [DllImport("kernel32.dll")]
    private static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, SafeFileHandle hSourceHandle, IntPtr hTargetProcessHandle, out SafeFileHandle lpTargetHandle, UInt32 dwDesiredAccess, Boolean bInheritHandle, UInt32 dwOptions);

    private const UInt32 ATTACH_PARENT_PROCESS = 0xFFFFFFFF;
    private const UInt32 STD_OUTPUT_HANDLE = 0xFFFFFFF5;
    private const UInt32 STD_ERROR_HANDLE = 0xFFFFFFF4;
    private const UInt32 DUPLICATE_SAME_ACCESS = 2;

    struct BY_HANDLE_FILE_INFORMATION
    {
      public UInt32 FileAttributes;
      public System.Runtime.InteropServices.ComTypes.FILETIME CreationTime;
      public System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
      public System.Runtime.InteropServices.ComTypes.FILETIME LastWriteTime;
      public UInt32 VolumeSerialNumber;
      public UInt32 FileSizeHigh;
      public UInt32 FileSizeLow;
      public UInt32 NumberOfLinks;
      public UInt32 FileIndexHigh;
      public UInt32 FileIndexLow;
    }

    public static void InitConsoleHandles()
    {
      SafeFileHandle hStdOut, hStdErr, hStdOutDup, hStdErrDup;
      BY_HANDLE_FILE_INFORMATION bhfi;
      hStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
      hStdErr = GetStdHandle(STD_ERROR_HANDLE);
      // Get current process handle
      IntPtr hProcess = Process.GetCurrentProcess().Handle;
      // Duplicate Stdout handle to save initial value
      DuplicateHandle(hProcess, hStdOut, hProcess, out hStdOutDup,
      0, true, DUPLICATE_SAME_ACCESS);
      // Duplicate Stderr handle to save initial value
      DuplicateHandle(hProcess, hStdErr, hProcess, out hStdErrDup,
      0, true, DUPLICATE_SAME_ACCESS);
      // Attach to console window – this may modify the standard handles
      AttachConsole(ATTACH_PARENT_PROCESS);
      // Adjust the standard handles
      if (GetFileInformationByHandle(GetStdHandle(STD_OUTPUT_HANDLE), out bhfi))
      {
        SetStdHandle(STD_OUTPUT_HANDLE, hStdOutDup);
      }
      else
      {
        SetStdHandle(STD_OUTPUT_HANDLE, hStdOut);
      }
      if (GetFileInformationByHandle(GetStdHandle(STD_ERROR_HANDLE), out bhfi))
      {
        SetStdHandle(STD_ERROR_HANDLE, hStdErrDup);
      }
      else
      {
        SetStdHandle(STD_ERROR_HANDLE, hStdErr);
      }
    }
  }
}
