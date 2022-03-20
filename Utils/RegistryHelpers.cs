﻿using Microsoft.Win32;
using System;

namespace RCPA.Utils
{
  public static class RegistryHelpers
  {
    public static RegistryKey GetRegistryKey()
    {
      return GetRegistryKey(null);
    }

    public static RegistryKey GetRegistryKey(string keyPath)
    {
      RegistryKey localMachineRegistry
          = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                    Environment.Is64BitOperatingSystem
                                        ? RegistryView.Registry64
                                        : RegistryView.Registry32);

      return string.IsNullOrEmpty(keyPath)
          ? localMachineRegistry
          : localMachineRegistry.OpenSubKey(keyPath);
    }

    public static object GetRegistryValue(string keyPath, string keyName)
    {
      RegistryKey registry = GetRegistryKey(keyPath);
      if (registry == null)
      {
        return null;
      }
      return registry.GetValue(keyName);
    }
  }
}
