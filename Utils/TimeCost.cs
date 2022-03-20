using System;

namespace RCPA.Utils
{
  public class TimeCost
  {
    private long _start;
    private long _end;
    private DateTime _startTime;

    public TimeCost(long start, long end)
    {
      this._start = start;
      this._end = end;
      this._startTime = DateTime.Now;
    }

    public void Reset()
    {
      _startTime = DateTime.Now;
    }

    public string GetCurrentDescription(long current)
    {
      TimeSpan duration = DateTime.Now.Subtract(_startTime);
      var requireTicks = new TimeSpan(duration.Ticks / current * (_end - current));
      var total = duration.Add(requireTicks);
      var finished = DateTime.Now.Add(requireTicks);
      return string.Format("cost {0}, estimated total cost {1}, will be finished around {2:MM/dd/yyyy H:mm:ss}",
              TimeSpanToString(ref duration),
              TimeSpanToString(ref total),
              finished);
    }


    public static string TimeSpanToString(ref TimeSpan duration)
    {
      string format;
      if (duration.Days > 0)
      {
        format = "{0:dd\\.hh\\:mm\\:ss}";
      }
      else if (duration.Hours > 0)
      {
        format = "{0:hh\\:mm\\:ss}";
      }
      else
      {
        format = "{0:mm\\:ss}";
      }

      return string.Format(format, duration);
    }

  }
}
