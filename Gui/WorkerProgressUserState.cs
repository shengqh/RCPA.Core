namespace RCPA.Gui
{
  public class WorkerProgressUserState
  {
    public WorkerProgressUserState(int labelIndex, string labelText)
    {
      this.IsProgress = false;
      this.LabelIndex = labelIndex;
      this.LabelText = labelText;
    }

    public WorkerProgressUserState(string labelText)
      : this(0, labelText)
    {
    }

    public WorkerProgressUserState(int progressBarIndex, long progressValue)
    {
      this.IsProgress = true;
      ProgressBarIndex = progressBarIndex;
      this.ProgressValue = progressValue;
    }

    public WorkerProgressUserState(int progressValue)
      : this(0, progressValue)
    {
    }

    public bool IsProgress { get; set; }

    public int LabelIndex { get; set; }

    public string LabelText { get; set; }

    public int ProgressBarIndex { get; set; }

    public long ProgressValue { get; set; }
  }
}