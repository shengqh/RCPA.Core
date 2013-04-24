namespace RCPA.Gui.Command
{
  public interface IToolCommand
  {
    string GetClassification();

    string GetCaption();

    string GetVersion();

    void Run();
  }
}