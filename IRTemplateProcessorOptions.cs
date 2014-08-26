using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA
{
  public interface IRTemplateProcessorOptions
  {
    string InputFile { get; set; }
    string OutputFile { get; set; }
    string RExecute { get; set; }
    string RTemplate { get; set; }
  }
}
