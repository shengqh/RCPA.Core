using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RCPA.Utils;

namespace RCPA.Gui
{
  public interface IProgress
  {
    IProgressCallback Progress { get; set; }
  }
}
