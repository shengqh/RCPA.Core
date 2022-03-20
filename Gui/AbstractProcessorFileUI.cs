using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class AbstractProcessorFileUI : AbstractProcessorBaseUI
  {
    public AbstractProcessorFileUI()
    {
      InitializeComponent();
    }

    protected override BackgroundWorker GetBackgroundWorker()
    {
      string sourceFile;
      try
      {
        sourceFile = GetOriginFile();
        if (null == sourceFile)
        {
          return null;
        }
      }
      catch (UserTerminatedException)
      {
        return null;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }

      IFileProcessor processor;
      try
      {
        processor = GetFileProcessor();
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }

      return new FileProcessorWorker(processor, sourceFile);
    }

    protected virtual IFileProcessor GetFileProcessor()
    {
      throw new NotImplementedException("GetFileProcessor");
    }

    protected virtual string GetOriginFile()
    {
      throw new NotImplementedException("GetOriginFile");
    }
  }
}
