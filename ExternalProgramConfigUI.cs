﻿using RCPA.Gui;
using RCPA.Gui.Command;
using RCPA.Gui.FileArgument;
using RCPA.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RCPA
{
  public partial class ExternalProgramConfigUI : ComponentUI
  {
    public static string title = "Extenal programs...";
    public static string version = "";

    private Dictionary<string, FileField> map = new Dictionary<string, FileField>();
    public ExternalProgramConfigUI()
    {
      InitializeComponent();

      rExecute.FileArgument = new OpenFileArgument("R Execute", "exe");
      map["r"] = rExecute;
    }

    protected override void OnAfterLoadOption(EventArgs e)
    {
      base.OnAfterLoadOption(e);
      if (string.IsNullOrWhiteSpace(rExecute.FullName) || !File.Exists(rExecute.FullName))
      {
        rExecute.FullName = SystemUtils.GetRExecuteLocation(false);
      }
    }

    public string GetExternalProgram(string programName)
    {
      var l = programName.ToLower();
      if (map.ContainsKey(l))
      {
        return map[l].FullName;
      }
      else
      {
        throw new ArgumentException("Cannot find definition of " + programName);
      }
    }

    public class Command : IToolCommand
    {
      public string GetClassification()
      {
        return MenuCommandType.Setup;
      }

      public string GetCaption()
      {
        return title;
      }

      public string GetVersion()
      {
        return version;
      }

      public void Run()
      {
        new ExternalProgramConfigUI().MyShowDialog();
      }
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      SaveOption();
      Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }
  }

  public static class ExternalProgramConfig
  {
    private static ExternalProgramConfigUI config = new ExternalProgramConfigUI();

    public static string GetExternalProgram(string programName)
    {
      config.LoadOption();

      var program = config.GetExternalProgram(programName);

      if (SystemUtils.IsLinux)
      {
        if (!string.IsNullOrWhiteSpace(program))
        {
          return program;
        }
        else
        {
          return programName;
        }
      }

      if (File.Exists(config.GetExternalProgram(programName)))
      {
        return config.GetExternalProgram(programName);
      }

      try
      {
        if (config.MyShowDialog() == DialogResult.OK)
        {
          var filename = config.GetExternalProgram(programName);
          if (File.Exists(filename))
          {
            return filename;
          }
        }
        else
        {
          throw new Exception("You may need to call Setup->Extenal programs to setup " + programName);
        }
      }
      catch (Exception)
      {
        throw new Exception("You may need to call Setup->Extenal programs to setup " + programName);
      }

      return null;
    }
  }
}
