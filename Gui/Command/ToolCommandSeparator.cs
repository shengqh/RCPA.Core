using System;

namespace RCPA.Gui.Command
{
  public class ToolCommandSeparator : IToolCommand
  {
    private readonly string menuCommand;

    public ToolCommandSeparator(string menuCommand)
    {
      this.menuCommand = menuCommand;
    }

    #region IToolCommand Members

    public string GetClassification()
    {
      return this.menuCommand;
    }

    public string GetCaption()
    {
      throw new NotImplementedException();
    }

    public string GetVersion()
    {
      throw new NotImplementedException();
    }

    public void Run()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}