using RCPA.Gui;
using System.Collections.Generic;
using System.ComponentModel;

namespace RCPA.Utils
{
  public class WorkerProgressCallback : AbstractProgressCallback
  {
    private class RangePosition
    {
      public RangePosition(long minimum, long maximum)
        : this(minimum, maximum, minimum)
      { }

      public RangePosition(long minimum, long maximum, long current)
      {
        this.minimum = minimum;
        this.maximum = maximum;
        this.current = current;
      }

      private long minimum;

      public long Minimum
      {
        get { return minimum; }
      }
      private long maximum;

      public long Maximum
      {
        get { return maximum; }
      }
      private long current;

      public long Current
      {
        get { return current; }
        set { current = value; }
      }

      public int Position
      {
        get
        {
          if (current <= minimum)
          {
            return 0;
          }

          if (current >= maximum)
          {
            return 100;
          }

          return (int)((current - minimum) * 100 / (maximum - minimum + 1));
        }
      }
    }

    private Dictionary<int, RangePosition> progressRange = new Dictionary<int, RangePosition>();

    public WorkerProgressCallback()
    { }

    public BackgroundWorker Worker { get; set; }

    #region IProgressCallback Members

    public override bool IsCancellationPending()
    {
      if (null != Worker)
      {
        return Worker.CancellationPending;
      }
      else
      {
        return false;
      }
    }

    public override void SetRange(int progressBarIndex, long minimum, long maximum)
    {
      progressRange[progressBarIndex] = new RangePosition(minimum, maximum);
    }

    public override void SetPosition(int progressBarIndex, long value)
    {
      if (null != Worker)
      {
        if (!progressRange.ContainsKey(progressBarIndex))
        {
          return;
        }

        var range = progressRange[progressBarIndex];
        if (range.Current != value)
        {
          var oldPosition = range.Position;
          range.Current = value;
          var curPosition = range.Position;
          if (oldPosition != curPosition)
          {
            Worker.ReportProgress(curPosition, new WorkerProgressUserState(progressBarIndex, curPosition));
          }
        }
      }
    }

    public override void Increment(int progressBarIndex, long value)
    {
      if (null != Worker)
      {
        if (!progressRange.ContainsKey(progressBarIndex))
        {
          return;
        }

        long lastProgressValue = progressRange[progressBarIndex].Current;
        SetPosition(progressBarIndex, lastProgressValue + value);
      }
    }

    public override void SetMessage(int labelIndex, string message)
    {
      if (null != Worker)
      {
        Worker.ReportProgress(0, new WorkerProgressUserState(labelIndex, message));
      }
    }

    public override void Begin()
    {
      foreach (int pIndex in progressRange.Keys)
      {
        SetPosition(pIndex, progressRange[pIndex].Minimum);
      }
    }

    public override void End()
    {
      foreach (int pIndex in progressRange.Keys)
      {
        SetPosition(pIndex, progressRange[pIndex].Maximum);
      }
    }

    public override bool IsConsole()
    {
      return false;
    }

    #endregion
  }
}
