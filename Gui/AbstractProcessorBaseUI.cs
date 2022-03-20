using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCPA.Gui
{
  public partial class AbstractProcessorBaseUI : AbstractUI
  {
    protected BackgroundWorker worker;

    public AbstractProcessorBaseUI()
    {
      InitializeComponent();

      base.AfterLoadOption += MyAfterLoadOption;
    }

    protected virtual bool IsProcessorSupportProgress()
    {
      return true;
    }

    protected virtual void MyAfterLoadOption(object sender, EventArgs e)
    {
      if (btnCancel.Visible != IsProcessorSupportProgress())
      {
        btnCancel.Visible = IsProcessorSupportProgress();
      }
    }

    protected virtual void ShowReturnInfo(IEnumerable<string> returnInfo)
    {
      if (null == returnInfo)
      {
        returnInfo = new List<string>();
      }

      var bIsFile = true;
      var sb = new StringBuilder();
      foreach (var file in returnInfo)
      {
        sb.Append("\n" + file);
        if (!File.Exists(file))
        {
          bIsFile = false;
        }
      }

      if (0 == returnInfo.Count())
      {
        MessageBox.Show(this, "Task finished.", "Congratulation", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      else if (bIsFile)
      {
        MessageBox.Show(this, "Result has saved to : " + sb, "Congratulation", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      else
      {
        MessageBox.Show(this, sb.ToString(), "Congratulation", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
    }

    private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      btnGo.Enabled = true;
      btnCancel.Enabled = false;
      btnClose.Enabled = true;

      if (e.Cancelled)
      {
        MessageBox.Show(this, "Task cancelled by user.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      else if (e.Error != null)
      {
        Console.Out.WriteLine(e.Error.StackTrace);
        MessageBox.Show(this, e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      else
      {
        var resultFiles = (IEnumerable<string>)e.Result;
        ShowReturnInfo(resultFiles);
      }
    }

    protected override void DoRealGo()
    {
      this.worker = GetBackgroundWorker();

      if (this.worker == null)
      {
        return;
      }

      this.worker.RunWorkerCompleted += RunWorkerCompleted;

      this.worker.ProgressChanged += GetProgressChanged();

      btnGo.Enabled = false;
      btnCancel.Enabled = true;
      btnClose.Enabled = false;

      this.worker.RunWorkerAsync();
    }

    protected virtual ProgressChangedEventHandler GetProgressChanged()
    {
      return new WorkerProgressChangedProxy(new[] { this.lblProgress }, new[] { this.progressBar }).ProgressChanged;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.worker.IsBusy)
      {
        this.worker.CancelAsync();
      }
    }

    protected virtual BackgroundWorker GetBackgroundWorker()
    {
      throw new NotImplementedException("GetBackgroundWorker");
    }
  }
}
