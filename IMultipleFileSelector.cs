using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA
{
  public interface IMultipleFileSelector
  {
    string[] FileNames { get; set; }

    string[] SelectedFileNames { get; set; }
  }
}
