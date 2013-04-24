using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
