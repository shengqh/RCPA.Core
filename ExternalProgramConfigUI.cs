using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RCPA.Gui;
using RCPA.Gui.FileArgument;
using System.IO;
using RCPA.Gui.Command;
using RCPA.Utils;

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
      if (string.IsNullOrWhiteSpace(rExecute.FullName))
      {
        rExecute.FullName = SystemUtils.GetRExecuteLocation();
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
        throw new ArgumentException("Cannot find definition of {0}", programName);
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

      if (File.Exists(config.GetExternalProgram(programName)))
      {
        return config.GetExternalProgram(programName);
      }

      while (config.MyShowDialog() == DialogResult.OK)
      {
        var name = config.GetExternalProgram(programName);
        if (File.Exists(name))
        {
          return name;
        }
      }

      return null;
    }
  }
}
