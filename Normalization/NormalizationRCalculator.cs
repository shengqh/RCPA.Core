using System;
using System.Linq;
using System.Collections.Generic;
using RCPA.Utils;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using RCPA.R;

namespace RCPA.Normalization
{
  public class NormalizationRCalculator : RTemplateProcessor
  {
    private NormalizationRCalculatorOptions options;

    public NormalizationRCalculator(NormalizationRCalculatorOptions options)
      : base(options)
    { }
  }
}