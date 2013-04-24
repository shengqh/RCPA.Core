using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ProtemomicsLib.Time
{
  public class QueryPerfCounter : ICounter
  {
    [DllImport("KERNEL32")]
    private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

    [DllImport("Kernel32.dll")]
    private static extern bool QueryPerformanceFrequency(out long lpFrequency);

    private long start;
    private DateTime startTime;

    private long stop;

    private long frequency;
    Decimal multiplier = new Decimal(1.0e3);

    public QueryPerfCounter()
    {
      if (QueryPerformanceFrequency(out frequency) == false)
      {
        // Frequency not supported 
        throw new Win32Exception();
      }
    }

    public void Start()
    {
      QueryPerformanceCounter(out start);
      startTime = DateTime.Now;
    }

    public DateTime Stop()
    {
      QueryPerformanceCounter(out stop);
      return EndTime;
    }

    public double Duration()
    {
      return ((((double)(stop - start) * (double)multiplier) / (double)frequency));
    }

    public DateTime EndTime
    {
      get { return startTime.AddMilliseconds(Duration()); }
    }

  }
}
