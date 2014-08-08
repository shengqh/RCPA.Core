using System;
using System.Collections.Generic;
using System.Text;

namespace RCPA.Utils
{
  public class ConsoleProgressCallback : AbstractProgressCallback
  {
    private long minimum;
    private long maximum;
    private long current;
    private long currentPercentage;

    public override bool IsCancellationPending()
    {
      return false;
    }

    public override void SetRange(long minimum, long maximum)
    {
      this.minimum = minimum;
      this.maximum = maximum;
      this.current = this.minimum;
      this.currentPercentage = 0;
    }

    public override void SetRange(int progressBarIndex, long minimum, long maximum)
    {
      SetRange(minimum, maximum);
    }

    public override void SetPosition(int progressBarIndex, long position)
    {
      if (position >= maximum)
      {
        position = maximum;
      }
      else if (position <= minimum)
      {
        position = minimum;
      }

      long newpercentage = position * 100 / (maximum - minimum);
      if (newpercentage != currentPercentage)
      {
        Console.Out.WriteLine(MyConvert.Format("{0}%", newpercentage));
        currentPercentage = newpercentage;
      }

      this.current = position;
    }

    public override void Increment(int progressBarIndex, long value)
    {
      SetPosition(this.current + value);
    }

    public override void SetMessage(int labelIndex, string message)
    {
      Console.WriteLine("{0:yyyyMMdd HH:mm:ss} - {1}", DateTime.Now, message);
    }

    public override void Begin() { }

    public override void End() { }
  }
}
