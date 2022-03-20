using System;

namespace ProtemomicsLib.Time
{
  public interface ICounter
  {
    void Start();

    DateTime Stop();

    double Duration();

    DateTime EndTime { get; }
  }
}
