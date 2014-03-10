using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using RCPA.Gui.FileArgument;

namespace RCPA.Gui
{
  public partial class AbstractProcessorUI : AbstractProcessorBaseUI
  {
    public AbstractProcessorUI()
    {
      InitializeComponent();
    }

    protected override BackgroundWorker GetBackgroundWorker()
    {
      IProcessor processor;
      try
      {
        processor = GetProcessor();
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }

      return new ProcessorWorker(processor);
    }

    protected virtual IProcessor GetProcessor()
    {
      throw new NotImplementedException("GetProcessor");
    }
  }
}
