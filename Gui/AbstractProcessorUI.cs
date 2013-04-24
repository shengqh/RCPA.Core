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
  public partial class AbstractProcessorUI : AbstractUI
  {
    protected BackgroundWorker worker;

    public AbstractProcessorUI()
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
      btnCancel.Visible = IsProcessorSupportProgress();
    }

    protected virtual void ShowReturnInfo(IEnumerable<string> returnInfo)
    {
      if (null == returnInfo)
      {
        returnInfo = new List<string>();
      }

      bool bIsFile = true;
      var sb = new StringBuilder();
      foreach (String file in returnInfo)
      {
        sb.Append("\n" + file);
        if (!new FileInfo(file).Exists)
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
      string sourceFile;
      try
      {
        sourceFile = GetOriginFile();
        if (null == sourceFile)
        {
          return;
        }
      }
      catch (UserTerminatedException)
      {
        return;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      IFileProcessor processor;
      try
      {
        processor = GetFileProcessor();
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      this.worker = new FileProcessorWorker(processor, sourceFile);

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

    protected virtual IFileProcessor GetFileProcessor()
    {
      throw new NotImplementedException("GetFileProcessor");
    }

    /// <summary>
    /// 获取目标文件名。子类可以继承该函数以自定义获取过程。
    /// 如果用户终止操作，例如未选择文件，那么返回null。
    /// </summary>
    /// <returns></returns>
    protected virtual string GetOriginFile()
    {
      throw new NotImplementedException("GetOriginFile");
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.worker.IsBusy)
      {
        this.worker.CancelAsync();
      }
    }
  }
}
