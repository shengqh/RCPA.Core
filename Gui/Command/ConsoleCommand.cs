using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA.Gui.Command
{
  public class ConsoleCommand : IToolCommand
  {
    #region IToolCommand Members

    public string GetClassification()
    {
      return "Option";
    }

    public string GetCaption()
    {
      return "Show console";
    }

    public string GetVersion()
    {
      return string.Empty;
    }

    public void Run()
    {
      WinFormConsole.Visible = !WinFormConsole.Visible;
      if (WinFormConsole.Visible)
      {
        WinFormConsole.DisableCloseButton();
      }
    }

    #endregion
  }
}
