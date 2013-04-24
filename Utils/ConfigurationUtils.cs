using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace RCPA.Utils
{
  public static class ConfigurationUtils
  {
    static public Configuration GetDefaultConfiguration()
    {
      return GetConfiguration(Application.ExecutablePath + ".config");
    }

    static public Configuration GetConfiguration(string configFilename)
    {
      ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
      fileMap.ExeConfigFilename = configFilename;
      return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
    }

    static public void ClearConfiguration(Configuration config)
    {
      string[] keys = config.AppSettings.Settings.AllKeys;
      foreach (string key in keys)
      {
        config.AppSettings.Settings.Remove(key);
      }
    }

    static public string GetValue(Configuration config, string key, string defaultValue)
    {
      if (config.HasFile)
      {
        foreach (string setkey in config.AppSettings.Settings.AllKeys)
        {
          if (setkey.Equals(key))
          {
            return config.AppSettings.Settings[key].Value;
          }
        }
      }

      return defaultValue;
    }

    static public bool GetValue(Configuration config, string key, bool defaultValue)
    {
      if (config.HasFile)
      {
        string value = GetValue(config, key, defaultValue.ToString());

        bool result;
        if (bool.TryParse(value, out result))
        {
          return result;
        }
      }

      return defaultValue;
    }

    static public int GetValue(Configuration config, string key, int defaultValue)
    {
      if (config.HasFile)
      {
        string value = GetValue(config, key, defaultValue.ToString());
        int result;
        if (int.TryParse(value, out result))
        {
          return result;
        }
      }

      return defaultValue;
    }

    static public double GetValue(Configuration config, string key, double defaultValue)
    {
      if (config.HasFile)
      {
        string value = GetValue(config, key, MyConvert.Format(defaultValue));

        double result;
        if (MyConvert.TryParse(value, out result))
        {
          return result;
        }
      }

      return defaultValue;
    }

    static public string[] GetList(Configuration config, string key, string splitPattern)
    {
      if (config.HasFile)
      {
        string value = GetValue(config, key, "");
        string[] parts = Regex.Split(value, splitPattern);
        List<string> result = new List<string>();
        foreach (string line in parts)
        {
          string trimLine = line.Trim();
          if (trimLine.Length != 0)
          {
            result.Add(trimLine);
          }
        }
        return result.ToArray();
      }
      else
      {
        return new string[0];
      }
    }

  }
}
