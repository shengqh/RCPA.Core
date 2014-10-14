using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LumenWorks.Framework.IO.Csv;
using System.Security.Cryptography;

namespace RCPA
{
  public static class FileUtils
  {
    public static FileStream OpenReadFile(string fileName)
    {
      return new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    }

    private static char[] pathchars = new char[] { '\\', '/' };

    public static string AppPath()
    {
      return AppDomain.CurrentDomain.BaseDirectory;
    }

    /// <summary>
    /// 替换最后一个后缀名。
    /// </summary>
    /// <param name="fileName">原文件名</param>
    /// <param name="extension">目标后缀，可以包含'.'，也可以不包含'.'，后者会自动添加'.'</param>
    /// <returns>结果文件名</returns>
    public static string ChangeExtension(string fileName, string extension)
    {
      if (extension == null)
      {
        throw new ArgumentNullException("extension");
      }

      if (extension.Length > 0 && !extension.StartsWith("."))
      {
        extension = "." + extension;
      }

      FileInfo fi = new FileInfo(fileName);
      if (fi.Extension.Length == 0)
      {
        return fileName + extension;
      }

      return fileName.Substring(0, fileName.Length - fi.Extension.Length) + extension;
    }

    /// <summary>
    /// 移除最后一个后缀名。
    /// </summary>
    /// <param name="fileName">原文件名</param>
    /// <returns>结果文件名</returns>
    public static string RemoveExtension(string fileName)
    {
      FileInfo fi = new FileInfo(fileName);
      if (fi.Extension.Length == 0)
      {
        return fileName;
      }

      return fileName.Substring(0, fileName.Length - fi.Extension.Length);
    }

    /// <summary>
    /// 移除所有后缀名，结果中不包含'.'。
    /// </summary>
    /// <param name="fileName">原文件名</param>
    /// <returns>结果文件名</returns>
    public static string RemoveAllExtension(string fileName)
    {
      int startIndex = fileName.LastIndexOfAny(pathchars);
      if (startIndex == -1)
      {
        startIndex = 0;
      }

      int pos = fileName.IndexOf('.', startIndex);
      if (-1 != pos)
      {
        return fileName.Substring(0, pos);
      }

      return fileName;
    }

    public static string GetFileName(string fullPath)
    {
      int startIndex = fullPath.LastIndexOfAny(pathchars);
      if (startIndex == -1)
      {
        return fullPath;
      }

      return fullPath.Substring(startIndex + 1);
    }

    public static List<string> ReadFile(string sFileName)
    {
      return ReadFile(sFileName, false);
    }

    public static List<string> ReadFile(string sFileName, bool skipEmptyLine)
    {
      List<string> result = new List<string>();
      using (StreamReader re = File.OpenText(sFileName))
      {
        String line;
        while ((line = re.ReadLine()) != null)
        {
          if (skipEmptyLine && (0 == line.Length))
          {
            continue;
          }

          result.Add(line);
        }
      }
      return result;
    }

    public static string ReadFileWithoutLineBreak(string sFileName, bool skipEmptyLine)
    {
      StringBuilder sb = new StringBuilder();

      using (StreamReader br = File.OpenText(sFileName))
      {
        String line;
        while ((line = br.ReadLine()) != null)
        {
          if (skipEmptyLine && line.Trim().Length == 0)
          {
            continue;
          }
          sb.Append(line);
        }
      }
      return sb.ToString();

    }

    public static void StringToFile(string sFileName, string sData)
    {
      using (StreamWriter sw = new StreamWriter(sFileName))
      {
        sw.WriteLine(sData);
      }
    }

    public static string FileToString(string sFileName)
    {
      using (StreamReader sr = new StreamReader(sFileName))
      {
        return sr.ReadToEnd();
      }
    }

    public static string StringToTempGUIDFile(string sData, string fileExtension)
    {
      //returns file name
      System.Guid g = System.Guid.NewGuid();
      string sFileOut = AppPath() + g + "." + fileExtension;
      StringToFile(sFileOut, sData);
      return sFileOut;
    }

    public static void CreateFile(string sFileName)
    {
      if (!File.Exists(sFileName))
      {
        StringToFile(sFileName, "");
      }
    }

    public static void DeleteFilter(string sPath, string sFilter)
    {
      System.IO.DirectoryInfo di = new DirectoryInfo(sPath);
      System.IO.FileInfo[] fArr = di.GetFiles(sFilter);
      foreach (System.IO.FileInfo f in fArr)
      {
        f.Delete();
      }
    }

    public static List<string> GetFiles(string[] sPaths, string sFilter)
    {
      return GetFiles(sPaths, sFilter, false);
    }

    public static List<string> GetFiles(string[] sPaths, string sFilter, bool drillDown)
    {
      HashSet<string> result = new HashSet<string>();
      foreach (string sPath in sPaths)
      {
        result.UnionWith(GetFiles(sPath, sFilter, drillDown));
      }

      return new List<string>(result);
    }

    public static List<string> GetFiles(string sPath)
    {
      return GetFiles(sPath, "", false);
    }

    public static List<string> GetFiles(string sPath, string[] sFilters)
    {
      return GetFiles(sPath, sFilters, false);
    }

    public static List<string> GetFiles(string sPath, string sFilter)
    {
      return GetFiles(sPath, sFilter, false);
    }

    public static List<string> GetFiles(string sPath, string[] sFilters, bool drillDown)
    {
      List<string> result = new List<string>();
      foreach (var filter in sFilters)
      {
        result.AddRange(GetFiles(sPath, filter, drillDown));
      }
      return result;
    }

    public static List<string> GetFiles(string sPath, string sFilter, bool drillDown)
    {
      DirectoryInfo di = new DirectoryInfo(sPath);

      FileInfo[] files;

      if (sFilter != null && sFilter.Length > 0)
      {
        files = di.GetFiles(sFilter);
      }
      else
      {
        files = di.GetFiles();
      }

      List<string> result = files.ToList().ConvertAll(f => f.FullName).ToList();

      if (drillDown == true)
      {
        foreach (DirectoryInfo d in di.GetDirectories())
        {
          result.AddRange(GetFiles(d.FullName, sFilter, drillDown));
        }
      }

      return result;
    }

    public static String GetConfigFile(Type type)
    {
      DirectoryInfo configDir = GetConfigDir();

      return configDir.FullName + "/" + type.Name + ".conf";
    }

    public static DirectoryInfo GetConfigDir()
    {
      DirectoryInfo result = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/config");
      if (!result.Exists)
      {
        result.Create();
      }
      return result;
    }

    public static DirectoryInfo GetTemplateDir()
    {
      DirectoryInfo result = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/template");
      if (!result.Exists)
      {
        result.Create();
      }
      return result;
    }

    public static String GetLogFile()
    {
      DirectoryInfo logDir = GetLogDir();

      return logDir.FullName + "/" + ChangeExtension(new FileInfo(Application.ExecutablePath).Name, ".log");
    }

    public static DirectoryInfo GetLogDir()
    {
      DirectoryInfo result = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/log");
      if (!result.Exists)
      {
        result.Create();
      }
      return result;
    }

    /// <summary>
    /// 获取Assembly的运行路径
    /// </summary>
    /// <returns></returns>
    public static string GetAssemblyPath()
    {
      string _CodeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

      _CodeBase = _CodeBase.Substring(8, _CodeBase.Length - 8);    // 8是 file:// 的长度

      string[] arrSection = _CodeBase.Split(new char[] { '/' });

      string _FolderPath = "";
      for (int i = 0; i < arrSection.Length - 1; i++)
      {
        _FolderPath += arrSection[i] + "/";
      }

      return _FolderPath;
    }

    public static string CreateDirectory(string parentDir, string name)
    {
      var result = parentDir + "/" + name;

      if (!Directory.Exists(result))
      {
        Directory.CreateDirectory(result);
      }

      return new DirectoryInfo(result).FullName;
    }

    public static string GetFullLinixName(string fileName)
    {
      return new FileInfo(fileName).FullName.Replace("\\", "/");
    }

    public static List<List<string>> ReadCsvFile(string fileName)
    {
      var result = new List<List<string>>();
      using (var sr = new StreamReader(fileName))
      {
        using (var reader = new CsvReader(sr, false))
        {
          while (reader.ReadNextRecord())
          {
            var parts = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
              parts.Add(reader[i]);
            }
            result.Add(parts);
          }
        }
      }
      return result;
    }

    public static string GetMd5HashForFile(MD5 md5Hash, string fileName)
    {
      using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        byte[] computedHashCode = md5Hash.ComputeHash(fs);
        return StringUtils.ByteToHexString(computedHashCode);
      }
    }

    public static string GetMd5HashForFile(string fileName)
    {
      using (var fun = MD5.Create())
      {
        return GetMd5HashForFile(fun, fileName);
      }
    }

    public static void DoGetRecursiveDirectories(string rootDir, List<string> result)
    {
      foreach (string d in Directory.GetDirectories(rootDir))
      {
        result.Add(d);
        DoGetRecursiveDirectories(d, result);
      }
    }

    public static List<string> GetRecursiveDirectories(string rootDir)
    {
      List<string> result = new List<string>();
      DoGetRecursiveDirectories(rootDir, result);
      return result;
    }
  }
}
