using RCPA.Utils;

namespace RCPA.Gui
{
  public class ProgressClass : IProgress
  {
    private IProgressCallback progress = new ConsoleProgressCallback();

    public IProgressCallback Progress
    {
      get
      {
        return this.progress;
      }
      set
      {
        if (value == null)
        {
          this.progress = new ConsoleProgressCallback();
        }
        else
        {
          this.progress = value;
        }
      }
    }
  }
}