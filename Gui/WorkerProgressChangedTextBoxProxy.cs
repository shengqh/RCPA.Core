using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace RCPA.Gui
{
  public class WorkerProgressChangedTextBoxProxy
  {
    private readonly TextBoxBase labels;
    private readonly ProgressBar[] progressBars;


    public WorkerProgressChangedTextBoxProxy(TextBoxBase labels, ProgressBar[] progressBars)
    {
      this.labels = labels;
      this.progressBars = progressBars;
    }

    public void ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (e.UserState is WorkerProgressUserState)
      {
        var eState = (WorkerProgressUserState)e.UserState;
        if (eState.IsProgress)
        {
          int progressBarIndex = Math.Min(eState.ProgressBarIndex, this.progressBars.Length - 1);
          var pb = this.progressBars[progressBarIndex];
          if (eState.ProgressValue >= 100)
          {
            pb.Value = 100;
          }
          else
          {
            pb.Value = (int)eState.ProgressValue;
          }
        }
        else
        {
          this.labels.AppendLine(eState.LabelText);
          this.labels.SelectionStart = this.labels.Text.Length;
          this.labels.ScrollToCaret();
        }
      }

      if (e.UserState is string)
      {
        this.labels.AppendLine((string)e.UserState);
        this.labels.SelectionStart = this.labels.Text.Length;
        this.labels.ScrollToCaret();
      }

      this.progressBars[0].Value = e.ProgressPercentage;
    }
  }
}