using RCPA.Utils;

namespace RCPA.Gui
{
  public interface IProgress
  {
    IProgressCallback Progress { get; set; }
  }
}
