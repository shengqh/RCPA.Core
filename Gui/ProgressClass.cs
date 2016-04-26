using RCPA.Utils;

namespace RCPA.Gui
{
  public class ProgressClass : IProgress
  {
    private IProgressCallback progress = new EmptyProgressCallback();

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
          this.progress = new EmptyProgressCallback();
        }
        else
        {
          this.progress = value;
        }
      }
    }
  }
}