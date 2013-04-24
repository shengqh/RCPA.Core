namespace RCPA.Gui
{
  public interface IRcpaFileDirectoryComponent : IDependentRcpaComponent
  {
    bool Exists { get; }

    string FullName { get; set; }

    void RemoveClickEvent();
  }
}