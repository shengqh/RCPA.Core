using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCPA.Normalization
{
  public class NormalizationRCalculatorOptions : IRTemplateProcessorOptions
  {
    public string InputFile { get; set; }

    public string OutputFile { get; set; }

    public string RExecute { get; set; }

    public string RTemplate { get; set; }
  }
}
