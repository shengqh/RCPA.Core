using System;

namespace RCPA.Gui.Command
{
  public class ToolSecondLevelCommandSeparator : IToolSecondLevelCommand
  {
    private readonly string menuCommand;
    private readonly string secondLevelCommand;

    public ToolSecondLevelCommandSeparator(string menuCommand, string secondLevelCommand)
    {
      this.menuCommand = menuCommand;
      this.secondLevelCommand = secondLevelCommand;
    }

    #region IToolSecondLevelCommand Members

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

    public string GetSecondLevelCommandItem()
    {
      return this.secondLevelCommand;
    }

    #endregion
  }
}